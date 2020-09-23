﻿using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [Route("api")]
    public class InstitutionController : ControllerBase
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        [HttpGet]
        [Route("institutions/{institutionId=0}")]
        public IActionResult GeInstitutions(int institutionId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _institutionRepository.GetInstitutions(institutionId, include, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPost]
        [Route("institutions")]
        public IActionResult Post(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.InsertInstitutions(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpPut]
        [Route("institutions")]
        public IActionResult Put(InstitutionsModel Model)
        {
            dynamic response = _institutionRepository.UpdateInstitution(Model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/{id}")]
        public IActionResult Delete(int id)
        {
            dynamic response = _institutionRepository.DeleteInstitution(id);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
