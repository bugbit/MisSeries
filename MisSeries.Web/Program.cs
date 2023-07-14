using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MisSeries.Web;
using MisSeries.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<TraktApi>();

builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

await builder.Build().RunAsync();
