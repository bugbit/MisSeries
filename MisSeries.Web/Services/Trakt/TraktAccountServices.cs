namespace MisSeries.Web.Services.Trakt;

public class TraktAccountServices
{
    private readonly TraktApi _traktApi;

    public TraktAccountServices(TraktApi traktApi)
    {
        _traktApi = traktApi;
    }

    public async Task LoginAsync(string code)
    {
        var response = await _traktApi.TokenAsync(code);

        if (string.IsNullOrEmpty(response.Token_type))
            throw new NullReferenceException(nameof(response.Token_type));
        if (string.IsNullOrEmpty(response.Access_token))
            throw new NullReferenceException(nameof(response.Access_token));

        _traktApi.SetAuthorization(response.Token_type, response.Access_token);
    }
}
