using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Helper.Abstraction
{
    public interface IServiceInstitutionIncludedRepository
    {
        dynamic GetServiceIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel);
        dynamic GetInstitutionsIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel);
    }
}
