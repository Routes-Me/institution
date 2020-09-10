using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IInstitutionRepository
    {
        dynamic InsertInstitutions(InstitutionsModel institutionModel);
        dynamic UpdateInstitution(InstitutionsModel institutionModel);
        dynamic DeleteInstitution(int id);
        dynamic GetInstitutions(int institutionId, string include, Pagination pageInfo);
    }
}
