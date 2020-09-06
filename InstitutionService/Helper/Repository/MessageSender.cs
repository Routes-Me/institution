using InstitutionService.Configuration;
using InstitutionService.Helper.Abstraction;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace InstitutionService.Helper.Repository
{
    public class MessageSender : IMessageSender
    {
        private readonly TwilioSMS _config;
        public MessageSender(TwilioSMS configuration)
        {
            _config = configuration;
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);
        }

        public async Task<MessageResource> SendMessage(string to, string body)
        {
            return await MessageResource.CreateAsync(new PhoneNumber("+91"+to),
                                              from: new PhoneNumber("+19418002903"),
                                              body: body);
        }
    }
}
