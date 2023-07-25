namespace MisSeries.Web.Services.Trakt.Request
{
    public class EpisodesRequest
    {
        public int? Number { get; set; }
        public int? Plays { get; set; }
        public DateTime? Last_watched_at { get; set; }
    }
}
