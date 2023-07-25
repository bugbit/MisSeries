namespace MisSeries.Web.Services.Trakt;

public class TokenData
{
    public required string TokenType { get; set; }
    public required string AccessToken { get; set; }
    public required string refreshToken { get; set; }
    public required DateTime ExpirateDate { get; set; }
    public required string Username { get; set; }
    public required string Slug { get; set; }
}
