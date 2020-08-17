using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Institutions
    {
        public Institutions()
        {
            Officers = new HashSet<Officers>();
            Servicesinstitutions = new HashSet<Servicesinstitutions>();
        }

        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PhoneNumber { get; set; }
        public string CountryIso { get; set; }

        public virtual ICollection<Officers> Officers { get; set; }
        public virtual ICollection<Servicesinstitutions> Servicesinstitutions { get; set; }
    }
}
