﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MisSeries.Web.Services.Trakt;
using System.Security.Claims;

namespace MisSeries.Web.Extensions.Authentication;

public class TraktAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly TraktApi _traktApi;

    public TraktAuthenticationStateProvider(ISessionStorageService sessionStorage, TraktApi traktApi)
    {
        _sessionStorage = sessionStorage;
        _traktApi = traktApi;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var storedToken = await _sessionStorage.GetItemAsync<TokenData>("token");
        ClaimsIdentity claimsIdentity;

        if (storedToken != null)
        {
            claimsIdentity = new ClaimsIdentity();

            // Crear ClaimsPrincipal con estos datos,
            // configurar encabezado de autorización por defecto,
            // y retornar un AuthenticationState() con el ClaimsPrincipal

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, storedToken.Username)
            };

            claimsIdentity = new ClaimsIdentity(claims, "trakt");
            _traktApi.SetAuthorization(storedToken.TokenType, storedToken.AccessToken);
        }
        else
            claimsIdentity = new ClaimsIdentity();

        var principal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(principal);
    }
}
