using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IInstitutionRepository
    {
        dynamic InsertInstitutions(InstitutionDto institutionModel);
        dynamic UpdateInstitution(InstitutionDto institutionModel);
        dynamic DeleteInstitution(string id);
        dynamic GetInstitutions(string institutionId, string include, Pagination pageInfo);
        dynamic GetInstitutionsOfficers(string institutionId, Pagination pageInfo);
    }
}
