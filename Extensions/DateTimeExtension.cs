using System;

namespace DatingAPI.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateTime refDateTime)
        {
            var years = DateTime.Today.Year - refDateTime.Year;
            if (refDateTime.AddYears(years) > DateTime.Today)
                years--;

            return years;
        }
    }
}