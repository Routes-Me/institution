using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IOfficersRepository
    {
        OfficersResponse InsertOfficers(int institutionsId, OfficersPostModel Model);
        OfficersResponse UpdateOfficers(int institutionsId, OfficersPostModel Model);
        OfficersResponse DeleteOfficers(int institutionsId, int officerId);
        OfficersGetResponse GetOfficers(int institutionsId, int officersId, PageInfo pageInfo); 
    }
}
    