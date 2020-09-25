using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [Route("api")]
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
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("services")]
        public IActionResult Put(ServicesModel Model)
        {
            dynamic response = _serviceRepository.UpdateService(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("services/{id}")]
        public IActionResult Delete(string id)
        {
            dynamic response = _serviceRepository.DeleteService(id);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]   
        [Route("services/{servicesId=0}")]
        public IActionResult Get(string servicesId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _serviceRepository.GetServices(servicesId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }
    }
}