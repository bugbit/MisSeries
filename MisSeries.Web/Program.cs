using System.Diagnostics.CodeAnalysis;
using Blazored.LocalStorage;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using MisSeries.Web;
using MisSeries.Web.Extensions.Authentication;
using MisSeries.Web.Model;
using MisSeries.Web.Services;
using MisSeries.Web.Services.Trakt;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(builder.HostEnvironment.IsProduction() ? LogLevel.None : LogLevel.Warning);

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddFileSystemAccessService();
builder.Services.AddScoped<TraktApi>();
builder.Services.AddScoped<AuthenticationStateProvider, TraktAuthenticationStateProvider>();
builder.Services.AddScoped<SyncRepository>();
builder.Services.AddScoped<SyncSqlLiteServices>();
builder.Services.AddScoped<TraktAccountServices>();
builder.Services.AddScoped<SyncServices>();

builder.Services.AddDbContext<MisSeriesDbContext>(
    options => options.UseSqlite("Data Source=misseries.sqlite3"));

builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

var host = builder.Build();

var dbService = host.Services.GetRequiredService<MisSeriesDbContext>();

// sqllite and Persisting data with the WebAssembly File System API
var syncService = host.Services.GetRequiredService<SyncSqlLiteServices>();

await syncService.InitFromOriginPrivateAsync(dbService);

await dbService.Database.EnsureCreatedAsync();

await host.RunAsync();

public partial class Program
{
    /// <summary>
    /// https://github.com/dotnet/efcore/issues/26860
    /// https://github.com/dotnet/aspnetcore/issues/39825
    /// FIXME: This is required for EF Core 6.0 as it is not compatible with trimming.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    private static Type _keepDateOnly = typeof(DateOnly);
}
