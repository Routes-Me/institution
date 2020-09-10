namespace InstitutionService.Models.DBModels
{
    public partial class ServicesInstitutions
    {
        public int InstitutionId { get; set; }
        public int ServiceId { get; set; }

        public virtual Institutions Institution { get; set; }
        public virtual Services Service { get; set; }
    }
}
