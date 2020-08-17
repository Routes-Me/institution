using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Services
    {
        public Services()
        {
            Servicesinstitutions = new HashSet<Servicesinstitutions>();
        }

        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Servicesinstitutions> Servicesinstitutions { get; set; }
    }
}
