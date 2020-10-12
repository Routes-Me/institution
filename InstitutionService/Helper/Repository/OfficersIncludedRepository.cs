using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Functions;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obfuscation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace InstitutionService.Helper.Repository
{
    public class OfficersIncludedRepository : IOfficersIncludedRepository
    {
        private readonly institutionserviceContext _context;
        private readonly AppSettings _appSettings;
        private readonly Dependencies _dependencies;
        public OfficersIncludedRepository(institutionserviceContext context, IOptions<AppSettings> appSettings, IOptions<Dependencies> dependencies)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _dependencies = dependencies.Value;

        }
        public dynamic GetInstitutionsIncludedData(List<OfficersModel> objOfficersModelList)
        {
            List<GetInstitutionsModel> lstInstitutions = new List<GetInstitutionsModel>();
            foreach (var item in objOfficersModelList)
            {
                var institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item.InstitutionId), _appSettings.PrimeInverse);
                var institutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institutionDetails != null)
                {
                    lstInstitutions.Add(new GetInstitutionsModel
                    {
                        InstitutionId = ObfuscationClass.EncodeId(institutionDetails.InstitutionId, _appSettings.Prime).ToString(),
                        Name = institutionDetails.Name,
                        CreatedAt = institutionDetails.CreatedAt,
                        PhoneNumber = institutionDetails.PhoneNumber,
                        CountryIso = institutionDetails.CountryIso
                    });
                }
            }
            var institutionsList = lstInstitutions.GroupBy(x => x.InstitutionId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(institutionsList.Cast<dynamic>().ToList());
        }

        public dynamic GetUsersIncludedData(List<OfficersModel> objOfficersModelList)
        {
            List<UserModel> lstUsers = new List<UserModel>();
            foreach (var item in objOfficersModelList)
            {
                var client = new RestClient(_appSettings.Host + _dependencies.UserUrl + item.UserId);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content;
                    var userData = JsonConvert.DeserializeObject<UserData>(result);
                    lstUsers.AddRange(userData.data);
                }
            }
            var usersList = lstUsers.GroupBy(x => x.UserId).Select(a => a.First()).ToList();
            return Common.SerializeJsonForIncludedRepo(usersList.Cast<dynamic>().ToList());
        }
    }
}
