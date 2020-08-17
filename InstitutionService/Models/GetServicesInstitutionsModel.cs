using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models
{
    public class GetServicesInstitutionsModel
    {
        public int InstitutionId { get; set; }
        public int ServiceId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
}
