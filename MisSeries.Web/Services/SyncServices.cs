using MisSeries.Web.Services.Trakt;

namespace MisSeries.Web.Services;

public class SyncServices
{
    private readonly TraktAccountServices _accountServices;
    private readonly TraktApi _traktApi;

    public SyncServices(TraktAccountServices accountServices, TraktApi traktApi)
    {
        _accountServices = accountServices;
        _traktApi = traktApi;
    }

    public async Task Sync(CancellationToken cancellationToken)
    {
        await SyncWatchShows(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
    }

    public async Task SyncWatchShows(CancellationToken cancellationToken)
    {
        await _accountServices.RefreshTokenIfNecesary(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var shows = await _traktApi.SyncWatchedShow(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
    }
}
