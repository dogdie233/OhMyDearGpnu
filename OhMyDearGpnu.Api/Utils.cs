namespace OhMyDearGpnu.Api
{
    public static class Utils
    {
        public static string GetCurrentMilliTimestamp() => (DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds.ToString("F0");
    }
}
