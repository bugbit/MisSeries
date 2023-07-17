using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MisSeries.Web;
using MisSeries.Web.Extensions.Authentication;
using MisSeries.Web.Services.Trakt;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<TraktApi>();
builder.Services.AddScoped<AuthenticationStateProvider, TraktAuthenticationStateProvider>();
builder.Services.AddScoped<TraktAuthenticationStateProvider, TraktAuthenticationStateProvider>();

builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
