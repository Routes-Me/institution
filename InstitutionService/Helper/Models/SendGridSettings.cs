namespace InstitutionService.Helper.Models
{
    public class SendGridSettings
    {
        public string APIKey { get; set; }
        public string From { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
