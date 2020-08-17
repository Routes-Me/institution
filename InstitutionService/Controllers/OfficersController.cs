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
        [Route("institutions/{institutionsId=0}/officers")]
        public IActionResult Post(int institutionsId, OfficersPostModel Model)
        {
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.InsertOfficers(institutionsId, Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpPut]
        [Route("institutions/{institutionsId=0}/officers")]
        public IActionResult Put(int institutionsId, OfficersPostModel Model)
        {
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.UpdateOfficers(institutionsId, Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("institutions/{institutionsId=0}/officers/{officersId}")]
        public IActionResult Delete(int institutionsId, int officersId) 
        {
            OfficersResponse response = new OfficersResponse();
            if (ModelState.IsValid)
                response = _officersRepository.DeleteOfficers(institutionsId, officersId);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("institutions/{institutionsId=0}/officers/{officersId=0}")]
        public IActionResult Get(int institutionsId, int officersId, [FromQuery] PageInfo pageInfo)
        {
            OfficersGetResponse response = new OfficersGetResponse();   
            response = _officersRepository.GetOfficers(institutionsId, officersId, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}