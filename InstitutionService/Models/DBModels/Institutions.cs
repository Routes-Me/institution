using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Institutions
    {
        public Institutions()
        {
            Officers = new HashSet<Officers>();
            ServicesInstitutions = new HashSet<ServicesInstitutions>();
        }

        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryIso { get; set; }

        public virtual ICollection<Officers> Officers { get; set; }
        public virtual ICollection<ServicesInstitutions> ServicesInstitutions { get; set; }
    }
}
