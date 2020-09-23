using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServiceRepository
    {
        dynamic InsertService(ServicesModel model);
        dynamic UpdateService(ServicesModel model);
        dynamic DeleteService(int id);
        dynamic GetServices(int servicesId, Pagination pageInfo);
    }
}
