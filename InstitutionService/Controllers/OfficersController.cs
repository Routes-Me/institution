using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
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
            return StatusCode(response.statusCode, response);
        }

        [HttpPut]
        [Route("officers")]
        public IActionResult Put(OfficersModel Model)
        {
            dynamic response = _officersRepository.UpdateOfficers(Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpDelete]
        [Route("officers/{officersId}")]
        public IActionResult Delete(string officersId)
        {
            dynamic response = _officersRepository.DeleteOfficers(officersId);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]
        [Route("officers/{officerId?}")]
        public IActionResult Get(string officerId, string userId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _officersRepository.GetOfficers(officerId, userId, include, pageInfo);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]
        [Route("officers/officerIds")]
        public IActionResult GetOfficerId(string userId)
        {
            GetOfficerIdResponse response = new GetOfficerIdResponse();
            try
            {
                response.OfficerId = _officersRepository.GetOfficerId(userId);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}