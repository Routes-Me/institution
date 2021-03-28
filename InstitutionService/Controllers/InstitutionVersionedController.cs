using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/")]
    public class InstitutionVersionedController : ControllerBase
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionVersionedController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        [HttpGet]
        [Route("institutions/{institutionId?}")]
        public IActionResult GeInstitutions(string institutionId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutions(institutionId, include, pageInfo);
            return StatusCode(response.statusCode, response);
        }

        [HttpPost]
        [Route("institutions")]
        public IActionResult Post(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.InsertInstitutions(Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpPut]
        [Route("institutions")]
        public IActionResult Put(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.UpdateInstitution(Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/{id}")]
        public IActionResult Delete(string id)
        {
            dynamic response = _institutionRepository.DeleteInstitution(id);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/{institutionId}/officers")]
        public IActionResult GetInstitutionsOfficers(string institutionId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutionsOfficers(institutionId, pageInfo);
            return StatusCode(response.statusCode, response);
        }
    }
}
