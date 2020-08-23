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
    public class ServicesInstitutionsController : BaseController
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
            ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
            if (ModelState.IsValid)
                response = _servicesInstitutionsRepository.InsertServicesInstitutions(institutionsId, Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        //[HttpPatch]
        //[Route("institutions/{institutionsId=0}/services")]
        //public IActionResult Put(int institutionsId, ServicesInstitutionsPostModel Model)
        //{
        //    ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
        //    if (ModelState.IsValid)
        //        response = _servicesInstitutionsRepository.UpdateServicesInstitutions(institutionsId, Model);
        //    if (response.responseCode != ResponseCode.Success)
        //        return GetActionResult(response);
        //    return Ok(response);
        //}

        [HttpDelete]
        [Route("institutions/{institutionsId=0}/services/{servicesId}")]
        public IActionResult Delete(int institutionsId, int servicesId)
        {
            ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
            if (ModelState.IsValid)
                response = _servicesInstitutionsRepository.DeleteServicesInstitutions(institutionsId, servicesId);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("institutions/{institutionId=0}/services/{servicesId=0}")]
        public IActionResult Get(int institutionId, int servicesId, string include, [FromQuery] PageInfo pageInfo)
        {
            ServicesInstitutionsGetResponse response = new ServicesInstitutionsGetResponse();
            response = _servicesInstitutionsRepository.GetServicesInstitutions(institutionId, servicesId, include,pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}   