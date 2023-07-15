using Microsoft.AspNetCore.Components;
using MisSeries.Web.Services.Trakt;

namespace MisSeries.Web.Components.Authorization;

public class TraktAuthorize : ComponentBase
{
    [Inject] protected ILogger<TraktAuthorize> _logger { get; set; }
    [Inject] protected NavigationManager NavManager { get; set; }
    [Inject] protected TraktApi TraktApi { get; set; }

    protected override void OnInitialized()
    {
        var returnUrl = NavManager.ToBaseRelativePath(NavManager.Uri);
        var uri = TraktApi.GetUrlAuthorize(returnUrl);

        //Console.WriteLine($"returnUrl={returnUrl} uri={uri}");
        
        _logger.LogInformation($"returnUrl={returnUrl} uri={uri}");

        NavManager.NavigateTo(uri);
    }
}
