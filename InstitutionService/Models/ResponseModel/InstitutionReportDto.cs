using System;

namespace InstitutionService.Internal.Dto
{
    public class InstitutionReportDto
    {
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryIso { get; set; }
    }

    public class InstitutionNameResponse
    {
        public string Name { get; set; }
    }
}
