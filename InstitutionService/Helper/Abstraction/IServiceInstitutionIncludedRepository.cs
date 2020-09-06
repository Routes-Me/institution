using InstitutionService.Models.ResponseModel;
using System.Collections.Generic;

namespace InstitutionService.Helper.Abstraction
{
    public interface IServiceInstitutionIncludedRepository
    {
        dynamic GetServiceIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel);
        dynamic GetInstitutionsIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel);
    }
}
