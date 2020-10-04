using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IAuthoritiesRepository
    {
        dynamic DeleteAuthorities(string id);
        dynamic GetAuthorities(string id, Pagination pageInfo);
        dynamic GetAuthoritiesByInstitutionId(string id, Pagination pageInfo);
        dynamic InsertAuthorities(AuthoritiesModel model);
        dynamic UpdateAuthorities(AuthoritiesModel model);
    }
}
