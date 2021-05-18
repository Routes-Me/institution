using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpPost]
        [Route("services")]
        public IActionResult Post(ServicesModel Model)
        {
            dynamic response = _serviceRepository.InsertService(Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpPut]
        [Route("services")]
        public IActionResult Put(ServicesModel Model)
        {
            dynamic response = _serviceRepository.UpdateService(Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpDelete]
        [Route("services/{id}")]
        public IActionResult Delete(string id)
        {
            dynamic response = _serviceRepository.DeleteService(id);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]   
        [Route("services/{serviceId?}")]
        public IActionResult Get(string serviceId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _serviceRepository.GetServices(serviceId, pageInfo);
            return StatusCode(response.statusCode, response);
        }
    }
}