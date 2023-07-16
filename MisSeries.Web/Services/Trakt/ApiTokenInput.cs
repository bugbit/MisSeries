﻿namespace MisSeries.Web.Services.Trakt;

public class ApiTokenInput
{
    public string? Code { get; set; }
    public string? RefreshToken { get; set; }
    public required string Client_id { get; set; }
    public required string Client_secret { get; set; }
    public required string Redirect_uri { get; set; }
    public required string Grant_type { get; set; }
}
