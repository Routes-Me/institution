using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace InstitutionService.Helper.Repository
{
    public class HelperRepository : IHelperRepository
    {
        private readonly SendGridSettings _sendGridSettings;
        public HelperRepository(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task<SendGrid.Response> SendEmail(string invitationLink, string email)
        {
            var client = new SendGridClient(_sendGridSettings.APIKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.From, _sendGridSettings.Name),
                Subject = _sendGridSettings.Subject,
                HtmlContent = "Invitation link: " + invitationLink
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);
            return await client.SendEmailAsync(msg);
        }
    }
}
