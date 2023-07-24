namespace MisSeries.Web.Services.Trakt.Request;

public class SyncWatchedShowShowRequest
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public SMOIdsRequest? Ids { get; set; }
}
