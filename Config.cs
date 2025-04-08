using System.Configuration;

namespace Speeron
{
    public static class Config
    {
        public static string BaseUrl => ConfigurationManager.AppSettings["BaseUrl"]
            ?? "https://reception.next-dev.speeron.com/recruitment-jakub-sobanski";

        public static string AuthToken =>
            Environment.GetEnvironmentVariable("AUTH_TOKEN")
            ?? ConfigurationManager.AppSettings["AuthToken"]
            ?? throw new Exception("Missing AUTH_TOKEN environment variable.");
        public static bool HeadlessMode => bool.Parse(ConfigurationManager.AppSettings["HeadlessMode"] ?? "false");
        public static int SlowMo => int.Parse(ConfigurationManager.AppSettings["SlowMo"] ?? "500");
        public static int Timeout => int.Parse(ConfigurationManager.AppSettings["Timeout"] ?? "30000");
    }
}