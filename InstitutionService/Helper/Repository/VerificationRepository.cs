using InstitutionService.Helper.Abstraction;
using InstitutionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;

namespace InstitutionService.Helper.Repository
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly Configuration.Twilio _config;

        public VerificationRepository(Configuration.Twilio configuration)
        {
            _config = configuration;
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);
        }
        public Task<Response> SendOTP(string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
