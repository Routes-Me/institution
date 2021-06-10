using System;

namespace InstitutionService.Models.ResponseModel
{
    public class DevicesDto
    {
        public string DeviceId { get; set; }
        public string SerialNumber { get; set; }
        public string SimSerialNumber { get; set; }
        public string VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
