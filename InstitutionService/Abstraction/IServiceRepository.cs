using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IServiceRepository
    {
        ServicesResponse InsertService(ServicesModel model);
        ServicesResponse UpdateService(ServicesModel model);
        ServicesResponse DeleteService(int id);
        ServicesGetResponse GetServices(int servicesId, PageInfo pageInfo);
    }
}
