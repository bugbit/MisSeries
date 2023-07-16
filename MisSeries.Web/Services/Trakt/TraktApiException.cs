using Microsoft.Extensions.Localization;
using MisSeries.Web.Constants;
using System.Runtime.Serialization;

namespace MisSeries.Web.Services.Trakt;

public class TraktApiException : ApplicationException
{
    public required int StatusCode { get; set; }
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

    public static TraktApiException CreateBadRequest(IStringLocalizer<TraktApiException> localizer, Exception? innerException = null)
        => new TraktApiException(localizer["BadRequest"], innerException) { StatusCode = StatusCodes.BadRequest, Error = "BadRequest" };
}
