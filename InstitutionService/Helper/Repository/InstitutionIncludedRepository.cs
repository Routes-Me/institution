using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Functions;
using InstitutionService.Helper.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obfuscation;
using System;
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
                var institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item.InstitutionId), _appSettings.PrimeInverse);
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
                                    ServiceId = ObfuscationClass.EncodeId(serviceData.ServiceId, _appSettings.Prime).ToString(),
                                    Name = serviceData.Name,
                                    Description = serviceData.Descriptions,
                                });
                            }
                        }
                    }
                }
            }
            var servicesList = lstServices.GroupBy(x => x.ServiceId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(servicesList.Cast<dynamic>().ToList());
        }
    }
}
