using Microsoft.AspNetCore.Components;

namespace MisSeries.Web.Components.Authorization;

public class Authorize : ComponentBase
{
    [Inject]
    protected NavigationManager NavManager { get; set; }

    protected override void OnInitialized()
    {
        var uri = NavManager.ToBaseRelativePath(NavManager.Uri);

        NavManager.NavigateTo("login?returnUrl=" + uri);
    }
}
