using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Functions;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Helper.Repository
{
    public class ServiceInstitutionIncludedRepository : IServiceInstitutionIncludedRepository
    {
        private readonly institutionserviceContext _context;
        public ServiceInstitutionIncludedRepository(institutionserviceContext context)
        {
            _context = context;
        }
        public dynamic GetInstitutionsIncludedData(List<ServicesInstitutionsModel> objServicesInstitutionsModel)
        {
            List<GetInstitutionsModel> lstInstitutions = new List<GetInstitutionsModel>();
            foreach (var item in objServicesInstitutionsModel)
            {
                var institutionsDetails = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(item.InstitutionId)).FirstOrDefault();
                if (institutionsDetails != null)
                {
                    lstInstitutions.Add(new GetInstitutionsModel
                    {
                        InstitutionId = institutionsDetails.InstitutionId.ToString(),
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
                var servicesDetails = _context.Services.Where(x => x.ServiceId == Convert.ToInt32(item.ServiceId)).FirstOrDefault();
                if (servicesDetails != null)
                {
                    lstServices.Add(new ServicesModel
                    {
                        ServiceId = servicesDetails.ServiceId.ToString(),
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
