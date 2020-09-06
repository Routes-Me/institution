using System.Threading.Tasks;

namespace InstitutionService.Helper.Abstraction
{
    public interface IHelperRepository
    {
        Task<SendGrid.Response> SendEmail(string invitationLink, string email);
    }
}
