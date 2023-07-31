using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MisSeries.Web.Extensions;

public static class DbContextExtensions
{
    public static string GetDataSource(this DatabaseFacade database)
    {
        var connection = database.GetDbConnection();

        return connection.DataSource;
    }
}