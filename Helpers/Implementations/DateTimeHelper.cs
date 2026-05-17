using MyPortfolio.Helpers.Interfaces;
using System.Globalization;

namespace MyPortfolio.Helpers.Implementations
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime Now => DateTime.Now;

        public string FormatMonthYear(DateTime date)
        {
            return date.ToString("MMM yyyy", CultureInfo.InvariantCulture);
        }

        public string FormatExperiencePeriod(DateTime startDate, DateTime? endDate, bool isCurrent)
        {
            var start = FormatMonthYear(startDate);

            if (isCurrent)
                return $"{start} - Present";

            if (endDate is null)
                return start;

            var end = FormatMonthYear(endDate.Value);

            return $"{start} - {end}";
        }
    }
}
