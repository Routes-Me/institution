﻿using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
    public class ServicesInstitutionsVersionedController : ControllerBase
    {
        private readonly IServicesInstitutionsRepository _servicesInstitutionsRepository;
        public ServicesInstitutionsVersionedController(IServicesInstitutionsRepository serviceInstitutionRepository)
        {
            _servicesInstitutionsRepository = serviceInstitutionRepository;
        }

        [HttpPost]
        [Route("institutions/{institutionId}/services")]
        public IActionResult Post(string institutionId, ServicesInstitutionsPostModel Model)
        {
            dynamic response = _servicesInstitutionsRepository.InsertServicesInstitutions(institutionId, Model);
            return StatusCode(response.statusCode, response);
        }

        [HttpDelete]
        [Route("institutions/{institutionId}/services/{serviceId}")]
        public IActionResult Delete(string institutionId, string serviceId)
        {
            dynamic response = _servicesInstitutionsRepository.DeleteServicesInstitutions(institutionId, serviceId);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]
        [Route("institutions/{institutionId}/services/{serviceId?}")]
        public IActionResult Get(string institutionId, string serviceId, string include, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _servicesInstitutionsRepository.GetServicesInstitutions(institutionId, serviceId, include, pageInfo);
            return StatusCode(response.statusCode, response);
        }
    }
}