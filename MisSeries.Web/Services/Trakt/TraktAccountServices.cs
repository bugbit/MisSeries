namespace MisSeries.Web.Services.Trakt;

public class TraktAccountServices
{
    private readonly TraktApi _traktApi;

    public TraktAccountServices(TraktApi traktApi)
    {
        _traktApi = traktApi;
    }

    public void Login(string code)
    {

    }
}
