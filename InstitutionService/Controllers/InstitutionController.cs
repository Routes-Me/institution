using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [Route("api")]
    public class InstitutionController : ControllerBase
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        [HttpGet]
        [Route("institutions/{institutionId?}")]
        public IActionResult GeInstitutions(string institutionId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutions(institutionId, include, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("v1/institutions/{institutionId?}")]
        public IActionResult GeInstitutionsV1(string institutionId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutions(institutionId, include, pageInfo);
            return StatusCode(response.statusCode, response);
        }

        [HttpPost]
        [Route("institutions")]
        public IActionResult Post(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.InsertInstitutions(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("institutions")]
        public IActionResult Put(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.UpdateInstitution(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/{id}")]
        public IActionResult Delete(string id)
        {
            dynamic response = _institutionRepository.DeleteInstitution(id);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/{institutionId}/officers")]
        public IActionResult GetInstitutionsOfficers(string institutionId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutionsOfficers(institutionId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
