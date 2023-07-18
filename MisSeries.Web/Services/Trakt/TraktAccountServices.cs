using MisSeries.Web.Extensions.Authentication;

namespace MisSeries.Web.Services.Trakt;

public class TraktAccountServices
{
    private readonly TraktApi _traktApi;
    private readonly TraktAuthenticationStateProvider _traktAuthenticationStateProvider;

    public TraktAccountServices(TraktApi traktApi, TraktAuthenticationStateProvider traktAuthenticationStateProvider)
    {
        _traktApi = traktApi;
        _traktAuthenticationStateProvider = traktAuthenticationStateProvider;
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
            RefreshType = response.Refresh_token ?? string.Empty,
            ExpirateDate = DateTime.Now.AddSeconds(response.Expires_in.GetValueOrDefault()),
            Username = responseUser.User?.Username ?? string.Empty,
            Slug = responseUser.User?.Ids?.Slug ?? string.Empty,
        };

        await _traktAuthenticationStateProvider.SetCurrentUserAsync(data, cancellationToken);
    }
}
