using System.Collections.Generic;
using InstitutionService.Internal.Dto;

namespace InstitutionService.Internal.Abstraction
{
    public interface IInstitutionsReportRepository
    {
        List<InstitutionReportDto> ReportInstitutions(List<int> institutionIds, List<string> attributes);
    }
}
