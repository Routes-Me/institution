using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [Route("api")]
    public class ServicesInstitutionsController : ControllerBase
    {
        private readonly IServicesInstitutionsRepository _servicesInstitutionsRepository;
        public ServicesInstitutionsController(IServicesInstitutionsRepository serviceInstitutionRepository)
        {
            _servicesInstitutionsRepository = serviceInstitutionRepository;
        }

        [HttpPost]
        [Route("institutions/{institutionsId=0}/services")]
        public IActionResult Post(int institutionsId, ServicesInstitutionsPostModel Model)
        {
            dynamic response = _servicesInstitutionsRepository.InsertServicesInstitutions(institutionsId, Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/{institutionsId=0}/services/{servicesId}")]
        public IActionResult Delete(int institutionsId, int servicesId)
        {
            dynamic response = _servicesInstitutionsRepository.DeleteServicesInstitutions(institutionsId, servicesId);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/{institutionId=0}/services/{servicesId=0}")]
        public IActionResult Get(int institutionId, int servicesId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _servicesInstitutionsRepository.GetServicesInstitutions(institutionId, servicesId, include, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }
    }
}   