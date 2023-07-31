namespace MisSeries.Web.Services;

public class SyncServices
{
    private readonly TraktAccountServices _accountServices;
    private readonly SyncRepository _syncRepository;
    private readonly TraktApi _traktApi;

    public SyncServices(TraktAccountServices accountServices, TraktApi traktApi, SyncRepository syncRepository)
    {
        _accountServices = accountServices;
        _traktApi = traktApi;
        _syncRepository = syncRepository;
    }


    public async Task ResetBDAndSync(CancellationToken cancellationToken = default)
    {
        await _syncRepository.ResetBD(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        await Sync(cancellationToken);
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
