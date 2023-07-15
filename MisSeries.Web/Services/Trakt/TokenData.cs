namespace MisSeries.Web.Services.Trakt;

public class TokenData
{
    public required string TokenType { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshType { get; set; }
    public required DateTime ExpirateDate { get; set; }
    public required string Username { get; set; }
}
