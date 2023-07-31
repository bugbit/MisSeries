using Microsoft.EntityFrameworkCore;
using MisSeries.Web.Services;

namespace MisSeries.Web.Model;

public class MisSeriesDbContext : DbContext
{
    private readonly SyncSqlLiteServices _syncSqlLiteServices;
    public MisSeriesDbContext(SyncSqlLiteServices syncSqlLiteServices, DbContextOptions options) : base(options)
    {
        _syncSqlLiteServices = syncSqlLiteServices;
    }

    protected MisSeriesDbContext(SyncSqlLiteServices syncSqlLiteServices)
    {
        _syncSqlLiteServices = syncSqlLiteServices;
    }

    // sqllite and Persisting data with the WebAssembly File System API
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        // sqllite and Persisting data with the WebAssembly File System API
        await _syncSqlLiteServices.PersistenceAsync(this, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return result;
    }
}