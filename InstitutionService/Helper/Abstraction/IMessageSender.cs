using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace InstitutionService.Helper.Abstraction
{
    public interface IMessageSender
    {
        //Task<Response> SendMessage(string to, string body);
        Task<MessageResource> SendMessage(string to, string body);
    }
}
