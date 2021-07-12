using System.Collections.Generic;

namespace InstitutionService.Abstraction
{
    public interface IInstitutionsReportRepository
    {
        dynamic ReportInstitutions(List<string> institutionIds, List<string> attributes);
    }
}
