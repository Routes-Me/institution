using InstitutionService.Models.ResponseModel;
using System.Collections.Generic;

namespace InstitutionService.Helper.Abstraction
{
    public interface IOfficersIncludedRepository
    {
        dynamic GetUsersIncludedData(List<OfficersModel> objOfficersModelList);
        dynamic GetInstitutionsIncludedData(List<OfficersModel> objOfficersModelList);
    }
}
