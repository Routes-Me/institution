using System;
using System.Collections.Generic;

namespace InstitutionService.Models.ResponseModel
{
    public class InstitutionsModel
    {
        public string InstitutionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryIso { get; set; }
        public List<string> services { get; set; }
    }

    public class GetInstitutionsModel
    {
        public string InstitutionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryIso { get; set; }
    }
}
