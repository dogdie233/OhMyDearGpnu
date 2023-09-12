namespace OhMyDearGpnu.Api.Test
{
    public class EncryptTests
    {
        private string exponentBase64 = null!;
        private string modulesBase64 = null!;

        [SetUp]
        public void Setup()
        {
            exponentBase64 = "AQAB";
            modulesBase64 = "AIKbje6TMrEOFaJTx1sGRVxvNxf6JHgX3CsSB6gOnUMXq9mbBEV+Xqo1YiVBlTQkAX6HP/cLrJk8ob05IB3jiLYd/ex3oHUg412yfQzvYduhJ1f9M8LvLZUAJf0FqFhBHZoJJ0uohH0BI7Ph+s5BnuJo8FhZ5y/uRgiDWiXLXSEb";
        }

        [Test]
        public void Test1()
        {
            var result = EncryptHelper.Encrypt("114514", exponentBase64, modulesBase64);
            Assert.Pass();
        }
    }
}