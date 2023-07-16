using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace MisSeries.Web.Services.Trakt
{
    // API Blueprint Viewer
    public class TraktApi
    {
        private static Lazy<string> _clientId = new(() => GetClientId());
        private static Lazy<string> _clientSecret = new(() => GetClientSecret());
        private readonly NavigationManager _navigationManager;
        private string? _authorization;

        public TraktApi(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void SetAuthorization(string token_type, string access_token)
        {
            _authorization = $"{token_type} {access_token}";
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
    }
}
