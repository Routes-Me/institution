using Microsoft.Extensions.Options;
using RoutesSecurity;
using System.Collections.Generic;
using System.Linq;
using InstitutionService.Abstraction;
using InstitutionService.Models.DBModels;
using InstitutionService.Helper.Models;
using InstitutionService.Models.ResponseModel;
using System.Data;

namespace InstitutionService.Repository
{
    public class InstitutionsReportRepository : IInstitutionsReportRepository
    {
        private readonly InstitutionsContext _context;
        private readonly AppSettings _appSettings;

        public InstitutionsReportRepository(IOptions<AppSettings> appSettings, InstitutionsContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public dynamic ReportInstitutions(List<string> institutionIds, List<string> attributes)
        {
            List<int> institutionIdsDecrypted = institutionIds.Select(v => Obfuscation.Decode(v)).ToList();

            return _context.Institutions
                .Where(v => institutionIdsDecrypted.Contains(v.InstitutionId))
                .Select(v => new InstitutionDto {
                    InstitutionId = Obfuscation.Encode(v.InstitutionId),
                    Name = attributes.Contains(nameof(v.Name)) ? v.Name : null,
                    PhoneNumber = attributes.Contains(nameof(v.PhoneNumber)) ? v.PhoneNumber : null,
                    CountryIso = attributes.Contains(nameof(v.CountryIso)) ? v.CountryIso : null,
                    CreatedAt = v.CreatedAt
                }).ToList();
        }
    }
}
