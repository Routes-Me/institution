using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IInvitationsRepository
    {
        Task<dynamic> InsertInvitation(string officerId, InvitationsModel model);
        dynamic DeleteInvitation(string officerId, string id);
        dynamic GetInvitation(string invitationId, Pagination pageInfo);
    }
}
