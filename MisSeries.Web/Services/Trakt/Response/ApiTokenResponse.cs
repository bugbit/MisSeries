namespace MisSeries.Web.Services.Trakt.Response;

public class ApiTokenResponse
{
    /*
   "access_token": "dbaf9757982a9e738f05d249b7b5b4a266b3a139049317c4909f2f263572c781",
                "token_type": "bearer",
                "expires_in": 7200, // secods
                "refresh_token": "76ba4c5c75c96f6087f58a4de10be6c00b29ea1ddc3b2022ee2016d1363e3a7c",
                "scope": "public",
                "created_at": 1487889741
    */
    public string? Access_token { get; set; }
    public string? Token_type { get; set; }
    /// <summary>
    /// en secods
    /// </summary>
    public int? Expires_in { get; set; }
    public string? Refresh_token { get; set; }
    public string? Scope { get; set; }
    public long? Created_at { get; set; }
}
