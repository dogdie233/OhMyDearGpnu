namespace OhMyDearGpnu.Api.Modules
{
    public class Calendar
    {
        public readonly DateTime firstWeekTime;

        public Calendar(DateTime firstWeekIn)
        {
            var date = firstWeekIn.Date;
            firstWeekTime = date - TimeSpan.FromDays((int)date.DayOfWeek);
        }

        public int CurrentWeek => GetWeek(DateTime.Now);
        public int GetWeek(DateTime date)
        {
            var week = (date.Date - firstWeekTime).TotalDays / 7;
            return (int)(week + (week >= 0 ? 1 : -1));
        }
    }
}
