using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models
{
    public class GetInstitutionModel
    {
        public int? institutionId { get; set; } = 0;
        public int? vehicleId { get; set; } = 0;
        public int? driverId { get; set; } = 0;
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public bool isInstitution { get; set; } = false;
        public bool isVehicle { get; set; } = false;
        public bool isDriver { get; set; } = false;
    }
}
