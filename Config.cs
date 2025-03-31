using System.Configuration;

namespace Speeron
{
    public static class Config
    {
        public static string BaseUrl => ConfigurationManager.AppSettings["BaseUrl"]
            ?? "https://reception.next-dev.speeron.com/recruitment-jakub-sobanski";
        /*
         This token should not be on github due to security.To prevent such leaks, you can additionally use tools such as:
            GitGuardian
            TruffleHog
            Gitleaks

        But because it is only a recruitment task, I decided to leave this fragment of code so that the person who will run it will not have to modify anything additionally in the code
        */
        public static string AuthToken => ConfigurationManager.AppSettings["AuthToken"]
            ?? "6M34foj4T45mCF2PGJv0t8ek7QWWpUN";
        public static bool HeadlessMode => bool.Parse(ConfigurationManager.AppSettings["HeadlessMode"] ?? "false");
        public static int SlowMo => int.Parse(ConfigurationManager.AppSettings["SlowMo"] ?? "500");
        public static int Timeout => int.Parse(ConfigurationManager.AppSettings["Timeout"] ?? "30000");
    }
}