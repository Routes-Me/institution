using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServiceRepository
    {
        dynamic InsertService(ServicesModel model);
        dynamic UpdateService(ServicesModel model);
        dynamic DeleteService(string id);
        dynamic GetServices(string serviceId, Pagination pageInfo);
    }
}
