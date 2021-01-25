using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IOfficersRepository
    {
        dynamic InsertOfficers(OfficersModel Model);
        dynamic UpdateOfficers(OfficersModel Model);
        dynamic DeleteOfficers(string officerId);
        dynamic GetOfficers(string officerId, string userId, string include, Pagination pageInfo);
    }
}
    