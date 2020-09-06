using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServicesInstitutionsRepository
    {
        ServicesInstitutionsResponse InsertServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel Model);
        ServicesInstitutionsResponse UpdateServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel model);
        ServicesInstitutionsResponse DeleteServicesInstitutions(int institutionsId, int servicesId);
        ServicesInstitutionsGetResponse GetServicesInstitutions(int institutionId, int servicesId, string include, Pagination pageInfo);
    }
}
