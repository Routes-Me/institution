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
        OfficersResponse InsertOfficers(OfficersModel Model);
        OfficersResponse UpdateOfficers(OfficersModel Model);
        OfficersResponse DeleteOfficers(int officerId);
        OfficersGetResponse GetOfficers(int officersId, PageInfo pageInfo); 
    }
}
    