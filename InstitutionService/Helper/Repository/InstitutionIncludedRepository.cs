using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Functions;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Helper.Repository
{
    public class InstitutionIncludedRepository : IInstitutionIncludedRepository
    {
        private readonly institutionserviceContext _context;
        public InstitutionIncludedRepository(institutionserviceContext context)
        {
            _context = context;
        }
        public dynamic GetServiceIncludedData(List<GetInstitutionsModel> objInstitutionsModelList)
        {
            List<ServicesModel> lstServices = new List<ServicesModel>();
            foreach (var item in objInstitutionsModelList)
            {
                var servicesInstitutionDetails = _context.ServicesInstitutions.Where(x => x.InstitutionId == item.InstitutionId).ToList();
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
                                    ServiceId = serviceData.ServiceId,
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
