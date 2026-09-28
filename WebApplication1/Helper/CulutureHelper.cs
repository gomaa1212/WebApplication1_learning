using System.Globalization;

namespace WebApplication1.Helper
{
    public static class CulutureHelper
    {
        public static bool IsRightToLeft()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            return currentCulture.TextInfo.IsRightToLeft;
        }
    }
}
