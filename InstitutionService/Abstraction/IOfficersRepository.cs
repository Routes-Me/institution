using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IOfficersRepository
    {
        dynamic InsertOfficers(OfficersModel Model);
        dynamic UpdateOfficers(OfficersModel Model);
        dynamic DeleteOfficers(int officerId);
        dynamic GetOfficers(int officersId,string include, Pagination pageInfo); 
    }
}
    