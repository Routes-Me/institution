namespace InstitutionService.Configuration
{

    public class TwilioSMS
    {
        public string AccountSid { get; set; } = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        public string AuthToken { get; set; } = "aXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    }

    public class PhoneNumbers
    {
        public static string Twilio { get; set; } = "+123456";
    }
}
