using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [Route("api")]
    [ApiController]
    public class OfficersController : ControllerBase
    {
        private readonly IOfficersRepository _officersRepository;
        public OfficersController(IOfficersRepository officersRepository)
        {
            _officersRepository = officersRepository;
        }

        [HttpPost]
        [Route("officers")]
        public IActionResult Post(OfficersModel Model)
        {
            dynamic response = _officersRepository.InsertOfficers(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("officers")]
        public IActionResult Put(OfficersModel Model)
        {
            dynamic response = _officersRepository.UpdateOfficers(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("officers/{officersId}")]
        public IActionResult Delete(string officersId)
        {
            dynamic response = _officersRepository.DeleteOfficers(officersId);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("officers/{officerId?}")]
        public IActionResult Get(string officerId, string userId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _officersRepository.GetOfficers(officerId, userId, include, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }


    }
}