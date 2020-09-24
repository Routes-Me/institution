using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServicesInstitutionsRepository
    {
        dynamic InsertServicesInstitutions(string institutionsId, ServicesInstitutionsPostModel Model);
        dynamic DeleteServicesInstitutions(string institutionsId, string servicesId);
        dynamic GetServicesInstitutions(string institutionId, string servicesId, string include, Pagination pageInfo);
    }
}
