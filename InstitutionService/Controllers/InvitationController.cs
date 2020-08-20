using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstitutionService.Controllers
{
    [Route("api")]
    [ApiController]
    public class InvitationController : BaseController
    {
        private readonly IInvitationsRepository _invitionRepository;
        public InvitationController(IInvitationsRepository invitionRepository)
        {
            _invitionRepository = invitionRepository;
        }

        [HttpPost]
        [Route("officer/{officerid=0}/{invitation}")]
        public async Task<IActionResult> Post(int officerid, InvitationsModel model)
        {
            InvitationsResponse response = new InvitationsResponse();
            if (ModelState.IsValid)
                response = await _invitionRepository.InsertInvitation(officerid, model);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("officer/{officerid=0}/{invitation}/{id}")]
        public IActionResult Delete(int officerid, int id)
        {
            InvitationsResponse response = new InvitationsResponse();
            if (ModelState.IsValid)
                response = _invitionRepository.DeleteInvitation(officerid, id);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("officer/{officerid=0}/{invitation}/{id=0}")]
        public IActionResult Get(int officerId, int id, [FromQuery] PageInfo pageInfo)
        {
            InvitationsGetResponse response = new InvitationsGetResponse();
            response = _invitionRepository.GetInvitation(officerId, id, pageInfo);
            if (response.responseCode != ResponseCode.Success)
                return GetActionResult(response);
            return Ok(response);
        }
    }
}
