namespace MyPortfolio.Helpers.Interfaces
{
    public interface IDateTimeHelper
    {
        DateTime Now { get; }

        string FormatMonthYear(DateTime date);

        string FormatExperiencePeriod(DateTime startDate, DateTime? endDate, bool isCurrent);
    }
}
