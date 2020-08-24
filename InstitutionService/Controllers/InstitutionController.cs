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
    [ApiController]
    [Route("api")]
    public class InstitutionController : BaseController
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        [HttpGet]
        [Route("institutions/{institutionId=0}")]
        public IActionResult GeInstitutions(int institutionId, [FromQuery] Pagination pageInfo)
        {
            InstitutionGetResponse response = new InstitutionGetResponse();
            response = _institutionRepository.GetInstitutions(institutionId, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpPost]
        [Route("institutions")]
        public IActionResult Post(InstitutionsModel Model)
        {
            InstitutionResponse response = new InstitutionResponse();
            if (ModelState.IsValid)
                response = _institutionRepository.InsertInstitutions(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpPut]
        [Route("institutions")]
        public IActionResult Put(InstitutionsModel Model)
        {
            InstitutionResponse response = new InstitutionResponse();
            if (ModelState.IsValid)
                response = _institutionRepository.UpdateInstitution(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("institutions/{id}")]
        public IActionResult Delete(int id)
        {
            InstitutionResponse response = new InstitutionResponse();
            if (ModelState.IsValid)
                response = _institutionRepository.DeleteInstitution(id);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}
