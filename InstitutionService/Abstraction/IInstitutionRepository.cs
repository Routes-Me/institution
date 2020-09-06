using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IInstitutionRepository
    {
        InstitutionResponse InsertInstitutions(InstitutionsModel institutionModel);
        InstitutionResponse UpdateInstitution(InstitutionsModel institutionModel);
        InstitutionResponse DeleteInstitution(int id);
        InstitutionGetResponse GetInstitutions(int institutionId, string include, Pagination pageInfo);
    }
}
