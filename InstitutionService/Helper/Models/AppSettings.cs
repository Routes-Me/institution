namespace InstitutionService.Helper.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string UserEndpointUrl { get; set; }
        public string InstitutionEndpointUrl { get; set; }
    }
}
