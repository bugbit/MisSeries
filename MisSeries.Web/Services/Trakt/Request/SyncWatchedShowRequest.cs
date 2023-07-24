namespace MisSeries.Web.Services.Trakt.Request
{
    public class SyncWatchedShowRequest
    {
        public int? Plays { get; set; }
        public DateTime? Last_watched_at { get; set; }
        public DateTime? Last_updated_at { get; set; }
        public DateTime? Reset_at { get; set; }
        public SyncWatchedShowShowRequest? Show { get; set; }
    }
}
