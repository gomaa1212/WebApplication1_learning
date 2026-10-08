using System.Globalization;

namespace WebApplication1.Helper
{
    public static class CultureHelper
    {
        public static bool IsRightToLeft()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            return currentCulture.TextInfo.IsRightToLeft;
        }
        public static bool IsEnglish()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            return currentCulture.TwoLetterISOLanguageName.ToLower() == "en";
        }
    }
}
