using System.Threading.Tasks;
using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [Route("api")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationsRepository _invitionRepository;
        public InvitationController(IInvitationsRepository invitionRepository)
        {
            _invitionRepository = invitionRepository;
        }

        [HttpPost]
        [Route("officers/{officerid=0}/invitations")]
        public async Task<IActionResult> Post(string officerid, InvitationsModel model)
        {
            dynamic response = await _invitionRepository.InsertInvitation(officerid, model);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpDelete]
        [Route("officers/{officerid=0}/{invitations}/{id}")]
        public IActionResult Delete(string officerid, string id)
        {
            dynamic response = _invitionRepository.DeleteInvitation(officerid, id);
            return StatusCode((int)response.statusCode, response);
        }

        [HttpGet]
        [Route("invitations/{invitationId=0}")]
        public IActionResult Get(string invitationId, [FromQuery] Pagination pageInfo)
        {
            dynamic response = _invitionRepository.GetInvitation(invitationId, pageInfo);
            return StatusCode((int)response.statusCode, response);
        }
    }
}
