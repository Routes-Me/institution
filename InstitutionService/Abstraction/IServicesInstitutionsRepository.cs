using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServicesInstitutionsRepository
    {
        dynamic InsertServicesInstitutions(string institutionId, ServicesInstitutionsPostModel Model);
        dynamic DeleteServicesInstitutions(string institutionId, string serviceId);
        dynamic GetServicesInstitutions(string institutionId, string serviceId, string include, Pagination pageInfo);
    }
}
