using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Abstraction
{
    public interface IOfficersRepository
    {
        OfficersResponse InsertOfficers(OfficersModel Model);
        OfficersResponse UpdateOfficers(OfficersModel Model);
        OfficersResponse DeleteOfficers(int officerId);
        OfficersGetResponse GetOfficers(int officersId,string include, Pagination pageInfo); 
    }
}
    