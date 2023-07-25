using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MisSeries.Web.Services.Trakt;
using System.Security;
using System.Security.Claims;

namespace MisSeries.Web.Extensions.Authentication;

public class TraktAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly TraktApi _traktApi;

    public TraktAuthenticationStateProvider(ILocalStorageService sessionStorage, TraktApi traktApi)
    {
        _localStorage = sessionStorage;
        _traktApi = traktApi;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var storedToken = await _localStorage.GetItemAsync<TokenData>("token");
        ClaimsIdentity claimsIdentity;

        if (storedToken != null)
        {
            claimsIdentity = CreateIdentity(storedToken);
            _traktApi.SetAuthorization(storedToken.TokenType, storedToken.AccessToken);
        }
        else
            claimsIdentity = new ClaimsIdentity();

        var principal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(principal);
    }

    private static ClaimsIdentity CreateIdentity(TokenData storedToken)
    {
        var claimsIdentity = new ClaimsIdentity();

        // Crear ClaimsPrincipal con estos datos,
        // configurar encabezado de autorización por defecto,
        // y retornar un AuthenticationState() con el ClaimsPrincipal

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, storedToken.Username),
            new Claim(ClaimTypes.NameIdentifier, storedToken.Slug),
            new Claim(ClaimTypes.Expiration, storedToken.ExpirateDate.ToShortTimeString())
        };

        claimsIdentity = new ClaimsIdentity(claims, "trakt");

        return claimsIdentity;
    }

    public async Task SetCurrentUserAsync(TokenData userData, CancellationToken cancellationToken)
    {
        await _localStorage.SetItemAsync("token", userData, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var claimsIdentity = CreateIdentity(userData);
        var principal = new ClaimsPrincipal(claimsIdentity);
        var authState = new AuthenticationState(principal);

        /*
		    * El método NotifyAuthenticationStateChanged(), proporcionado por la clase base, notifica a todos los componentes suscritos del cambio de estado,
		    * a la vez que les envía la tarea (en este caso, resuelta) para obtener el estado de autenticación actual.
		*/
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task ClearCurrentUser(CancellationToken cancellationToken)
    {
        var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var authStateTask = Task.FromResult(new AuthenticationState(anonymousPrincipal));

        await _localStorage.RemoveItemAsync("token", cancellationToken);
        /*
         * El método NotifyAuthenticationStateChanged(), proporcionado por la clase base, notifica a todos los componentes suscritos del cambio de estado,
         * a la vez que les envía la tarea (en este caso, resuelta) para obtener el estado de autenticación actual.
         */
        NotifyAuthenticationStateChanged(authStateTask);
    }

    public async Task<(DateTime expirateDate, string refreshToken)> GetDataForRefreshToken(CancellationToken cancellationToken)
    {
        var data = await _localStorage.GetItemAsync<TokenData>("token", cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (data == null)
            throw new SecurityException("Token borrado");

        return (data.ExpirateDate, data.refreshToken);
    }

    public async Task RefreshToken(string accessToken, string refreshToken, DateTime expirateDate, CancellationToken cancellationToken)
    {
        var data = await _localStorage.GetItemAsync<TokenData>("token", cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (data == null)
            throw new SecurityException("Token borrado");

        data.AccessToken = accessToken;
        data.refreshToken = refreshToken;
        data.ExpirateDate = expirateDate;

        await _localStorage.SetItemAsync("token", data, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
    }
}
