using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MisSeries.Web.Services.Trakt.Request;
using MisSeries.Web.Services.Trakt.Response;

namespace MisSeries.Web.Services.Trakt
{
    // API Blueprint Viewer
    public class TraktApi
    {
        private static Lazy<string> _clientId = new(() => GetClientId());
        private static Lazy<string> _clientSecret = new(() => GetClientSecret());
        private readonly NavigationManager _navigationManager;
        private readonly IStringLocalizer _Localizer;
        private HttpClient _httpClient;

        public TraktApi(NavigationManager navigationManager, IStringLocalizer<TraktApi> localizer)
        {
            _navigationManager = navigationManager;
            CreateHttpClient();
            _Localizer = localizer;
        }

        public void SetAuthorization(string token_type, string access_token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token_type, access_token);
            _httpClient.DefaultRequestHeaders.Add("trakt-api-version", "2");
            _httpClient.DefaultRequestHeaders.Add("trakt-api-key", _clientId.Value);
        }

        public string GetUrlAuthorize(string redirect_uri)
            => _navigationManager.GetUriWithQueryParameters
                (
                    "https://trakt.tv/oauth/authorize",
                    new Dictionary<string, object?>
                    {
                        ["client_id"] = _clientId.Value,
                        ["redirect_uri"] = redirect_uri,
                        ["response_type"] = "code"
                    }
                );

        public async Task<TokenResponse> TokenAsync(string code, CancellationToken cancellationToken)
        {
            var uri = new Uri(_httpClient.BaseAddress!, "/oauth/token");
            var response = await _httpClient.PostAsJsonAsync(uri, new TokenRequest
            {
                Client_id = _clientId.Value,
                Client_secret = _clientSecret.Value,
                Code = code,
                Redirect_uri = _navigationManager.ToAbsoluteUri("login").ToString(),
                Grant_type = GrantTypes.AuthorizationCode
            }, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TokenResponse>() ?? new();

            throw TraktApiException.CreateByStatusCode(response.StatusCode, _Localizer);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var uri = new Uri(_httpClient.BaseAddress!, "/oauth/token");
            var response = await _httpClient.PostAsJsonAsync(uri, new TokenRequest
            {
                Client_id = _clientId.Value,
                Client_secret = _clientSecret.Value,
                Refresh_token = refreshToken,
                Redirect_uri = _navigationManager.ToAbsoluteUri("login").ToString(),
                Grant_type = GrantTypes.RefreshToken
            }, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? new();

                // after read api, reset de httpClient so the followers fetch send cors errors!
                _httpClient.CancelPendingRequests();
                _httpClient.Dispose();
                CreateHttpClient();

                return result;
            }

            throw TraktApiException.CreateByStatusCode(response.StatusCode, _Localizer);
        }

        public async Task<UsersSettingsRequest> UsersSettingsAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri(_httpClient.BaseAddress!, "/users/settings");
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UsersSettingsRequest>() ?? new();

            throw TraktApiException.CreateByStatusCode(response.StatusCode, _Localizer);
        }


        public async Task<SyncWatchedShowRequest[]> SyncWatchedShow(CancellationToken cancellationToken)
        {
            var uri = new Uri(_httpClient.BaseAddress!, "/sync/watched/shows");
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<SyncWatchedShowRequest[]>() ?? new SyncWatchedShowRequest[0];

            throw TraktApiException.CreateByStatusCode(response.StatusCode, _Localizer);
        }

        unsafe static private string GetClientId()
        {
            var data = new byte[80];
            var maxlen = data.Length;
            int size;

            fixed (byte* ptr = data)
            {
                size = getClientId((nint)ptr, maxlen);
            }

            var str = Encoding.UTF8.GetString(data, 0, size);

            return str;
        }

        unsafe static private string GetClientSecret()
        {
            var data = new byte[80];
            var maxlen = data.Length;
            int size;

            fixed (byte* ptr = data)
            {
                size = getClientSecret((nint)ptr, maxlen);
            }

            var str = Encoding.UTF8.GetString(data, 0, size);

            return str;
        }

        [DllImport("Trakt")]
        static extern int getClientId(nint data, int maxlen);
        [DllImport("Trakt")]
        static extern int getClientSecret(nint data, int maxlen);

        private void CreateHttpClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.trakt.tv")
            };
        }
    }
}
