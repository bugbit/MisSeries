using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MisSeries.Web.Extensions.Authentication;

public class TraktAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;

    public TraktAuthenticationStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //var storedToken = await _sessionStorage.GetItemAsync<UserData>("token");

        await Task.CompletedTask;

        return null;
    }
}
