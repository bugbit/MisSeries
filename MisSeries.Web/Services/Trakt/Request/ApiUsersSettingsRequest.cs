namespace MisSeries.Web.Services.Trakt.Request;

public class ApiUsersSettingsRequest
{
    public class JSUser
    {
        public string? Username { get; set; }
    }

    public class JSIds
    {
        public string? Slug { get; set; }
    }

    public class JSImages
    {
        public class JSAvatar
        {
            public string? Full { get; set; }
        }

        public JSAvatar? Avatar { get; set; }
    }

    public JSUser? User { get; set; }
    public JSIds? Ids { get; set; }
    public JSImages? Images { get; set; }
}
