using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Helper.Abstraction
{
    public interface IInstitutionIncludedRepository
    {
        dynamic GetServiceIncludedData(List<GetInstitutionsModel> objInstitutionsModelList);
    }
}
