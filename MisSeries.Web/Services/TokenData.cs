namespace MisSeries.Web.Services;

public class TokenData
{
    public required string TypeToken { get; set; }
    public required string Token { get; set; }
    public required string RefreshType { get; set; }
    public required DateTime ExpirateDate { get; set; }
}
