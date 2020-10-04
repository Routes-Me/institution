using System.Collections.Generic;
using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthoritiesController : ControllerBase
    {
        private readonly IAuthoritiesRepository _authoritiesRepository;
        public AuthoritiesController(IAuthoritiesRepository authoritiesRepository)
        {
            _authoritiesRepository = authoritiesRepository;
        }

        [HttpPost]
        [Route("institutions/authorities")]
        public IActionResult Post(AuthoritiesModel model)
        {
            dynamic response = _authoritiesRepository.InsertAuthorities(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/authorities/{id=0}")]
        public IActionResult Get(string id, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _authoritiesRepository.GetAuthorities(id, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/{institutionId}/authorities")]
        public IActionResult GetAuthoritiesByInstitution(string institutionId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _authoritiesRepository.GetAuthoritiesByInstitutionId(institutionId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("institutions/authorities")]
        public IActionResult Put(AuthoritiesModel model)
        {
            dynamic response = _authoritiesRepository.UpdateAuthorities(model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/authorities/{id}")]
        public IActionResult Delete(string id)
        {
            dynamic response = _authoritiesRepository.DeleteAuthorities(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
