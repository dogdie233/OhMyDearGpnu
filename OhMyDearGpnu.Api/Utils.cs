namespace OhMyDearGpnu.Api
{
    internal static class Utils
    {
        public static string GetCurrentMilliTimestamp() => (DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds.ToString("F0");
    }
}
