using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServicesInstitutionsRepository
    {
        dynamic InsertServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel Model);
        dynamic DeleteServicesInstitutions(int institutionsId, int servicesId);
        dynamic GetServicesInstitutions(int institutionId, int servicesId, string include, Pagination pageInfo);
    }
}
