namespace InstitutionService.Models.ResponseModel
{
    public class ServicesInstitutionsModel
    {
        public string InstitutionId { get; set; }
        public string ServiceId { get; set; }
    }

    public class ServicesInstitutionsPostModel
    {
        public string id { get; set; }
        public string ServiceId { get; set; }
    }
}