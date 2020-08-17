using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class DriversModel
    {
        public int DriverId { get; set; }
        public int? UserId { get; set; }
        public int? InstitutionId { get; set; }
    }
}
