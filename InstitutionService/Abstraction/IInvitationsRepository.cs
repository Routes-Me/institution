using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IInvitationsRepository
    {
        Task<dynamic> InsertInvitation(int officerId, InvitationsModel model);
        dynamic DeleteInvitation(int officerId, int id);
        dynamic GetInvitation(int invitationId, Pagination pageInfo);
    }
}
