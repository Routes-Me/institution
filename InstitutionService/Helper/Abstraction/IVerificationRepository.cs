using InstitutionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Helper.Abstraction
{
    public interface IVerificationRepository
    {
        Task<Response> SendOTP(string phoneNumber);
    }
}
