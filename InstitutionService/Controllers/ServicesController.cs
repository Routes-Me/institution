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
    public class ServicesController : BaseController
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
            ServicesResponse response = new ServicesResponse();
            if (ModelState.IsValid)
                response = _serviceRepository.InsertService(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpPut]
        [Route("services")]
        public IActionResult Put(ServicesModel Model)
        {
            ServicesResponse response = new ServicesResponse();
            if (ModelState.IsValid)
                response = _serviceRepository.UpdateService(Model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("services/{id}")]
        public IActionResult Delete(int id)
        {
            ServicesResponse response = new ServicesResponse();
            if (ModelState.IsValid)
                response = _serviceRepository.DeleteService(id);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpGet]   
        [Route("services/{servicesId=0}")]
        public IActionResult Get(int servicesId, [FromQuery] PageInfo pageInfo)
        {
            ServicesGetResponse response = new ServicesGetResponse();
            response = _serviceRepository.GetServices(servicesId, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}