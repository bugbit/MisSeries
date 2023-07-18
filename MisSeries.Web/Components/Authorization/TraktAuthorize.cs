using Microsoft.AspNetCore.Components;
using MisSeries.Web.Services.Trakt;

namespace MisSeries.Web.Components.Authorization;

public class TraktAuthorize : ComponentBase
{
    [Inject] protected TraktAccountServices TraktAccountSrv { get; set; }

    protected override void OnInitialized() => TraktAccountSrv.Authorize();
}
