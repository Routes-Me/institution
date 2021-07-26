using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using InstitutionService.Internal.Abstraction;
using InstitutionService.Models.DBModels;
using InstitutionService.Helper.Models;
using InstitutionService.Internal.Dto;
using System.Data;

namespace InstitutionService.Internal.Repository
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

        public List<InstitutionReportDto> ReportInstitutions(List<int> institutionIds, List<string> attributes)
        {
            return _context.Institutions
                .Where(v => institutionIds.Contains(v.InstitutionId))
                .Select(v => new InstitutionReportDto {
                    InstitutionId = v.InstitutionId,
                    Name = attributes.Contains(nameof(v.Name)) ? v.Name : null,
                    PhoneNumber = attributes.Contains(nameof(v.PhoneNumber)) ? v.PhoneNumber : null,
                    CountryIso = attributes.Contains(nameof(v.CountryIso)) ? v.CountryIso : null,
                    CreatedAt = v.CreatedAt
                }).ToList();
        }
    }
}
