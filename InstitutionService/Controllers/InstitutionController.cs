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
        public IActionResult GeInstitutions(int institutionId, [FromQuery] PageInfo pageInfo)
        {
            InstitutionGetResponse response = new InstitutionGetResponse();
            response = _institutionRepository.GetInstitutions(institutionId, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        //[HttpGet]
        //[Route("institutions/{institutionId=0}/{vehicles}/{vehicleId=0}")]
        //public IActionResult GetVehicles(int institutionId, int vehicleId, [FromQuery] PageInfo pageInfo)
        //{
        //    InstitutionVehicleResponse response = new InstitutionVehicleResponse();
        //    response = _institutionRepository.GetVehicles(institutionId, vehicleId, pageInfo);
        //    if (response.responseCode != ResponseCode.Success)
        //        return GetActionResult(response);
        //    return Ok(response);
        //}

        //[HttpGet]
        //[Route("institutions/{institutionId=0}/vehicles/{vehicleId=0}/drivers/{driverId=0}")]
        //public IActionResult GetDrivers(int institutionId, int vehicleId, int driverId, [FromQuery] PageInfo pageInfo)
        //{
        //    InstitutionDriverResponse response = new InstitutionDriverResponse();
        //    response = _institutionRepository.GetDrivers(institutionId, vehicleId, driverId, pageInfo);
        //    if (response.responseCode != ResponseCode.Success)
        //        return GetActionResult(response);
        //    return Ok(response);
        //}

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
