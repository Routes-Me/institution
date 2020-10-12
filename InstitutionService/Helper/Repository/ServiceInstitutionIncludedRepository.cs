using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Functions;
using InstitutionService.Helper.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.Extensions.Options;
using Obfuscation;
using System;
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
                var institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item.InstitutionId), _appSettings.PrimeInverse);
                var institutionsDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institutionsDetails != null)
                {
                    lstInstitutions.Add(new GetInstitutionsModel
                    {
                        InstitutionId = ObfuscationClass.EncodeId(institutionsDetails.InstitutionId, _appSettings.Prime).ToString(),
                        Name = institutionsDetails.Name,
                        CreatedAt = institutionsDetails.CreatedAt,
                        PhoneNumber = institutionsDetails.PhoneNumber,
                        CountryIso = institutionsDetails.CountryIso
                    });
                }
            }
            var institutionsList = lstInstitutions.GroupBy(x => x.InstitutionId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(institutionsList.Cast<dynamic>().ToList());
        }

        public dynamic GetServiceIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel)
        {
            List<ServicesModel> lstServices = new List<ServicesModel>();
            foreach (var item in objServicesInstitutionsModel)
            {
                var serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item.InstitutionId), _appSettings.PrimeInverse);
                var servicesDetails = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
                if (servicesDetails != null)
                {
                    lstServices.Add(new ServicesModel
                    {
                        ServiceId = ObfuscationClass.EncodeId(servicesDetails.ServiceId, _appSettings.Prime).ToString(),
                        Name = servicesDetails.Name,
                        Description = servicesDetails.Descriptions,
                    });
                }
            }
            var servicesList = lstServices.GroupBy(x => x.ServiceId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(servicesList.Cast<dynamic>().ToList());
        }
    }
}
