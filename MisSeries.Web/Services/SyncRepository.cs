namespace MisSeries.Web.Services;

public class SyncRepository
{
    private readonly MisSeriesDbContext _dbContext;
    private readonly SyncSqlLiteServices _syncSqlLiteServices;

    public SyncRepository(MisSeriesDbContext dbContext, SyncSqlLiteServices syncSqlLiteServices)
    {
        _dbContext = dbContext;
        _syncSqlLiteServices = syncSqlLiteServices;
    }

    public async Task ResetBD(CancellationToken cancellationToken = default)
    {
        await _syncSqlLiteServices.EnsureDeletedAsync(_dbContext, cancellationToken);
    }
}