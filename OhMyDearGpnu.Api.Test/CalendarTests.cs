using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Test
{
    public class CalendarTests
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TodayStart()
        {
            var date = new DateTime(2023, 9, 5);
            var calendar = new Calendar(date);
            var week1 = calendar.GetWeek(date);
            Assert.That(week1 == 1);
        }

        [Test]
        public void NextDay()
        {
            var date = new DateTime(2023, 9, 6);
            var calendar = new Calendar(date);
            var week1 = calendar.GetWeek(date);
            Assert.That(week1 == 1);
        }

        [Test]
        public void StartAtSunday()
        {
            var calendar = new Calendar(new DateTime(2023, 9, 3));
            var week2 = calendar.GetWeek(new DateTime(2023, 9, 10));
            Assert.That(week2 == 2);
        }

        [Test]
        public void BeforeTermBegins()
        {
            var calendar = new Calendar(new DateTime(2023, 9, 3));
            var week啊 = calendar.GetWeek(new DateTime(2023, 9, 2));
            Assert.That(week啊 == -1);
        }
    }
}
