using System.Threading.Tasks;
using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
    public class InvitationVersionedController : ControllerBase
    {
        private readonly IInvitationsRepository _invitionRepository;
        public InvitationVersionedController(IInvitationsRepository invitionRepository)
        {
            _invitionRepository = invitionRepository;
        }

        [HttpPost]
        [Route("officers/{officerid=0}/invitations")]
        public async Task<IActionResult> Post(string officerid, InvitationsModel model)
        {
            dynamic response = await _invitionRepository.InsertInvitation(officerid, model);
            return StatusCode(response.statusCode, response);
        }

        [HttpDelete]
        [Route("officers/{officerid=0}/{invitations}/{id}")]
        public IActionResult Delete(string officerid, string id)
        {
            dynamic response = _invitionRepository.DeleteInvitation(officerid, id);
            return StatusCode(response.statusCode, response);
        }

        [HttpGet]
        [Route("invitations/{invitationId?}")]
        public IActionResult Get(string invitationId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _invitionRepository.GetInvitation(invitationId, pageInfo);
            return StatusCode(response.statusCode, response);
        }
    }
}
