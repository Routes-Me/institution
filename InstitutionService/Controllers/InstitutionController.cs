using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
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
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [Route("institutions")]
        public IActionResult Post(InstitutionDto Model)
        {
            dynamic response = _institutionRepository.InsertInstitutions(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("institutions")]
        public IActionResult Put(InstitutionDto Model)
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
