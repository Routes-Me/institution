using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IServicesInstitutionsRepository
    {
        ServicesInstitutionsResponse InsertServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel Model);
        ServicesInstitutionsResponse UpdateServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel model);
        ServicesInstitutionsResponse DeleteServicesInstitutions(int institutionsId, int servicesId);
        ServicesInstitutionsGetResponse GetServicesInstitutions(int institutionId, int servicesId, string include, PageInfo pageInfo);
    }
}
