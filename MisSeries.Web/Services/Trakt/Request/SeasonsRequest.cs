namespace MisSeries.Web.Services.Trakt.Request;

public class SeasonsRequest
{
    public int? Number { get; set; }
    public EpisodesRequest[]? Episodes { get; set; }
}
