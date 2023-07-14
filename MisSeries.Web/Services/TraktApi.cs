using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;

namespace MisSeries.Web.Services
{
    // API Blueprint Viewer
    public class TraktApi
    {
        unsafe public string Prueba()
        {
            var data = new byte[80];
            var maxlen = data.Length;
            int size;

            fixed (byte* ptr = data)
            {
                size = getClientId((IntPtr)ptr, maxlen);
            }

            var str = Encoding.UTF8.GetString(data, 0, size);

            return str;
        }
        [DllImport("Trakt")]
        static extern int getClientId(IntPtr data, int maxlen);
    }
}
