using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [Route("api")]
    [ApiController]
    public class OfficersController : BaseController
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
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.InsertOfficers(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpPut]
        [Route("officers")]
        public IActionResult Put(OfficersModel Model)
        {
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.UpdateOfficers(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("officers/{officersId}")]
        public IActionResult Delete(int officersId) 
        {
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.DeleteOfficers(officersId);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("officers/{officersId=0}")]
        public IActionResult Get(int officersId,string include, [FromQuery] PageInfo pageInfo)
        {
            OfficersGetResponse response = new OfficersGetResponse();
            response = _officersRepository.GetOfficers(officersId, include, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}