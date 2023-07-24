namespace MisSeries.Web.Services.Trakt.Request;

/*
 * "ids": {
        "trakt": 127287,
        "slug": "titans-2018",
        "tvdb": 341663,
        "imdb": "tt1043813",
        "tmdb": 75450,
        "tvrage": null
      }
 */
public class SMOIdsRequest
{
    public int? Trakt { get; set; }
    public string? Slung { get; set; }
    public int? Tvdb { get; set; }
    public int? Imdb { get; set; }
    public int? Tmdb { get; set; }
    public int? Tvrage { get; set; }
}
