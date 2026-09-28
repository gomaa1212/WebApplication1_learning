using System.Globalization;

namespace WebApplication1.Helper
{
    public class LocalizableEntity
    {
        public string localize(string nameEn,string nameAr)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            if (currentCulture.TwoLetterISOLanguageName.ToLower() == "ar")
            {
                return nameAr;
            }
            return nameEn;
        }
    }
}
