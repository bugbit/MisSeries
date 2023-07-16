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
        var urlLogin = NavManager.ToAbsoluteUri("login").ToString();
        var returnUrl = NavManager.Uri;
        var redirect_uri = NavManager.GetUriWithQueryParameters
        (
            urlLogin,
            new Dictionary<string, object?>
            { ["returnUrl"] = returnUrl }
        );
        var uri = TraktApi.GetUrlAuthorize(returnUrl);

        NavManager.NavigateTo(uri);
    }
}
