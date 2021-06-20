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
    public class InstitutionIncludedRepository : IInstitutionIncludedRepository
    {
        private readonly institutionserviceContext _context;
        private readonly AppSettings _appSettings;

        public InstitutionIncludedRepository(IOptions<AppSettings> appSettings, institutionserviceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public dynamic GetServiceIncludedData(List<InstitutionsModel> objInstitutionsModelList)
        {
            List<ServicesModel> lstServices = new List<ServicesModel>();
            foreach (var item in objInstitutionsModelList)
            {
                var institutionIdDecrypted = Obfuscation.Decode(item.InstitutionId);
                var servicesInstitutionDetails = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionIdDecrypted).ToList();
                if (servicesInstitutionDetails != null && servicesInstitutionDetails.Count > 0)
                {
                    foreach (var serviceDetails in servicesInstitutionDetails)
                    {
                        var servicesDetails = _context.Services.Where(x => x.ServiceId == serviceDetails.ServiceId).ToList();
                        if (servicesDetails != null && servicesDetails.Count > 0)
                        {
                            foreach (var serviceData in servicesDetails)
                            {
                                lstServices.Add(new ServicesModel
                                {
                                    ServiceId = Obfuscation.Encode(serviceData.ServiceId),
                                    Name = serviceData.Name,
                                    Description = serviceData.Descriptions,
                                });
                            }
                        }
                    }
                }
            }
            return lstServices.GroupBy(x => x.ServiceId).Select(a => a.First()).ToList();
        }
    }
}
