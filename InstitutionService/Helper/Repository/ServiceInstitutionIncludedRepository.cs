using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.Extensions.Options;
using RoutesSecurity;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Helper.Repository
{
    public class ServiceInstitutionIncludedRepository : IServiceInstitutionIncludedRepository
    {
        private readonly institutionserviceContext _context;
        private readonly AppSettings _appSettings;

        public ServiceInstitutionIncludedRepository(IOptions<AppSettings> appSettings, institutionserviceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public dynamic GetInstitutionsIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel)
        {
            List<GetInstitutionsModel> lstInstitutions = new List<GetInstitutionsModel>();
            foreach (var item in objServicesInstitutionsModel)
            {
                var institutionIdDecrypted = Obfuscation.Decode(item.InstitutionId);
                var institutionsDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institutionsDetails != null)
                {
                    lstInstitutions.Add(new GetInstitutionsModel
                    {
                        InstitutionId = Obfuscation.Encode(institutionsDetails.InstitutionId),
                        Name = institutionsDetails.Name,
                        CreatedAt = institutionsDetails.CreatedAt,
                        PhoneNumber = institutionsDetails.PhoneNumber,
                        CountryIso = institutionsDetails.CountryIso
                    });
                }
            }
            return lstInstitutions.GroupBy(x => x.InstitutionId).Select(a => a.First()).ToList();
        }

        public dynamic GetServiceIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel)
        {
            List<ServicesModel> lstServices = new List<ServicesModel>();
            foreach (var item in objServicesInstitutionsModel)
            {
                var serviceIdDecrypted = Obfuscation.Decode(item.InstitutionId);
                var servicesDetails = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
                if (servicesDetails != null)
                {
                    lstServices.Add(new ServicesModel
                    {
                        ServiceId = Obfuscation.Encode(servicesDetails.ServiceId),
                        Name = servicesDetails.Name,
                        Description = servicesDetails.Descriptions,
                    });
                }
            }
            return lstServices.GroupBy(x => x.ServiceId).Select(a => a.First()).ToList();
        }
    }
}
