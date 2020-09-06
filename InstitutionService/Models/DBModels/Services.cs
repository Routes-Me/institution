using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Services
    {
        public Services()
        {
            ServicesInstitutions = new HashSet<ServicesInstitutions>();
        }

        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }

        public virtual ICollection<ServicesInstitutions> ServicesInstitutions { get; set; }
    }
}
