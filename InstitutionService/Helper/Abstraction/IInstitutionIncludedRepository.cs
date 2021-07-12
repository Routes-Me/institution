using InstitutionService.Models.ResponseModel;
using System.Collections.Generic;

namespace InstitutionService.Helper.Abstraction
{
    public interface IInstitutionIncludedRepository
    {
        dynamic GetServiceIncludedData(List<InstitutionDto> objInstitutionsModelList);
    }
}
