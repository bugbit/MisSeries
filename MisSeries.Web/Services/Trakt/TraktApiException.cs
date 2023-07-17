using Microsoft.Extensions.Localization;
using MisSeries.Web.Constants;
using System.Net;
using System.Runtime.Serialization;

namespace MisSeries.Web.Services.Trakt;

public class TraktApiException : ApplicationException
{
    public required HttpStatusCode StatusCode { get; set; }
    public required string Error { get; set; }

    public TraktApiException()
    {
    }

    public TraktApiException(string? message) : base(message)
    {
    }

    public TraktApiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TraktApiException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public static TraktApiException CreateByStatusCode(HttpStatusCode statusCode, IStringLocalizer localizer, Exception? innerException = null)
        => statusCode switch
        {
            HttpStatusCode.BadRequest => new TraktApiException(localizer["BadRequest"], innerException) { StatusCode = statusCode, Error = "BadRequest" },
            HttpStatusCode.InternalServerError => new TraktApiException(localizer["ServerError"], innerException) { StatusCode = statusCode, Error = "ServerError" },
            HttpStatusCode.BadGateway or HttpStatusCode.ServiceUnavailable or HttpStatusCode.GatewayTimeout => new TraktApiException(localizer["ServiceUnavailable"], innerException) { StatusCode = statusCode, Error = "ServiceUnavailable" },
            _ => new TraktApiException(localizer["CallApiError"], innerException) { StatusCode = statusCode, Error = "Error" }
        };
}
