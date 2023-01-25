using System.Collections.Generic;
using InstitutionService.Internal.Dto;

namespace InstitutionService.Internal.Abstraction
{
    public interface IInstitutionsReportRepository
    {
        string GetName(int institutionId);
        List<InstitutionReportDto> ReportInstitutions(List<int> institutionIds, List<string> attributes);
    }
}
