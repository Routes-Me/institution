using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IInvitationsRepository
    {
        Task<InvitationsResponse> InsertInvitation(int officerId, InvitationsModel model);
        InvitationsResponse DeleteInvitation(int officerId, int id);
        InvitationsGetResponse GetInvitation(int invitationId, Pagination pageInfo);
    }
}
