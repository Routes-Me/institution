using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IServiceRepository
    {
        ServicesResponse InsertService(ServicesModel model);
        ServicesResponse UpdateService(ServicesModel model);
        ServicesResponse DeleteService(int id);
        ServicesGetResponse GetServices(int servicesId, Pagination pageInfo);
    }
}
