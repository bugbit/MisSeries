using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;

namespace MisSeries.Web.Services.Trakt
{
    // API Blueprint Viewer
    public class TraktApi
    {
        private string? _authorization { get; set; }

        public void SetAuthorization(string token_type, string access_token)
        {
            _authorization = $"{token_type} {access_token}";
        }

        unsafe public string Prueba()
        {
            var data = new byte[80];
            var maxlen = data.Length;
            int size;

            fixed (byte* ptr = data)
            {
                size = _getClientId((nint)ptr, maxlen);
            }

            var str = Encoding.UTF8.GetString(data, 0, size);

            return str;
        }

        [DllImport("Trakt")]
        static extern int _getClientId(nint data, int maxlen);
    }
}
