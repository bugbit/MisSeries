using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MisSeries.Web.Extensions.Authentication;
using MisSeries.Web.Services.Trakt.Response;

namespace MisSeries.Web.Services.Trakt;

public class TraktAccountServices
{
    private readonly TraktApi _traktApi;
    private readonly TraktAuthenticationStateProvider _traktAuthenticationStateProvider;
    private readonly NavigationManager _navigationManager;

    public TraktAccountServices(TraktApi traktApi, AuthenticationStateProvider traktAuthenticationStateProvider, NavigationManager navigationManager)
    {
        _traktApi = traktApi;
        _traktAuthenticationStateProvider = (TraktAuthenticationStateProvider)traktAuthenticationStateProvider;
        _navigationManager = navigationManager;
    }

    public void Authorize()
    {
        var urlLogin = _navigationManager.ToAbsoluteUri("login").ToString();
        //var returnUrl = NavManager.Uri;
        //var redirect_uri = NavManager.GetUriWithQueryParameters
        //(
        //    urlLogin,
        //    new Dictionary<string, object?>
        //    { ["returnUrl"] = returnUrl }
        //);
        //var uri = TraktApi.GetUrlAuthorize(returnUrl);
        var uri = _traktApi.GetUrlAuthorize(urlLogin);

        _navigationManager.NavigateTo(uri);
    }

    public async Task LoginAsync(string code, CancellationToken cancellationToken)
    {
        var response = await _traktApi.TokenAsync(code, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(response.Token_type))
            throw new NullReferenceException(nameof(response.Token_type));
        if (string.IsNullOrEmpty(response.Access_token))
            throw new NullReferenceException(nameof(response.Access_token));

        _traktApi.SetAuthorization(response.Token_type, response.Access_token);

        var responseUser = await _traktApi.UsersSettingsAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var data = new TokenData
        {
            AccessToken = response.Access_token,
            TokenType = response.Token_type,
            refreshToken = response.Refresh_token ?? string.Empty,
            ExpirateDate = DateTime.Now.AddSeconds(response.Expires_in.GetValueOrDefault()),
            Username = responseUser.User?.Username ?? string.Empty,
            Slug = responseUser.User?.Ids?.Slug ?? string.Empty,
        };

        await _traktAuthenticationStateProvider.SetCurrentUserAsync(data, cancellationToken);

        GoHome();
    }

    public async Task LogOffAsync(CancellationToken cancellationToken)
    {
        await _traktAuthenticationStateProvider.ClearCurrentUser(cancellationToken);

        GoHome();
    }

    public async Task<bool> RefreshTokenIfNecesary(CancellationToken cancellationToken)
    {
        var data = await _traktAuthenticationStateProvider.GetDataForRefreshToken(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var diff = data.expirateDate - DateTime.Now;

        if (diff.Days > 1)
            return false;

        var response = await _traktApi.RefreshTokenAsync(data.refreshToken, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        _traktApi.SetAuthorization(response.Token_type!, response.Access_token!);
        await _traktAuthenticationStateProvider.RefreshToken(response.Access_token!, response.Refresh_token!, DateTime.Now.AddSeconds(response.Expires_in.GetValueOrDefault()), cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return true;
    }

    private void GoHome()
    {
        var uri = _navigationManager.ToAbsoluteUri("").ToString();

        _navigationManager.NavigateTo(uri);
    }
}
