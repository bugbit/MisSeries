namespace MisSeries.Services;

public class TraktApi
{
    private HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.trakt.tv") };

    public required string ClientId { get; set; }


    public TraktApi()
    {
        _httpClient.DefaultRequestHeaders.Add("trakt-api-key", ClientId);
        _httpClient.DefaultRequestHeaders.Add("trakt-api-version", "2");
    }

    public Task<string>GenerateNewDeviceCode()
    {
        _httpClient.PostAsync
    }
}
