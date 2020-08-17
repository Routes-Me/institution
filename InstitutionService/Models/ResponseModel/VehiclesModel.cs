using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class VehiclesModel
    {
        public int VehicleId { get; set; }
        public int? DeviceId { get; set; }
        public string PlateNumber { get; set; }
        public int? InstitutionId { get; set; }
        public int ModelYear { get; set; }
        public int? ModelId { get; set; }
    }
}
