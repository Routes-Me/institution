using InstitutionService.Abstraction;
using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;

namespace InstitutionService.Repository
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IInstitutionIncludedRepository _institutionIncludedRepository;
        private readonly AppSettings _appSettings;
        private readonly Dependencies _dependencies;

        public InstitutionRepository(IOptions<AppSettings> appSettings, IOptions<Dependencies> dependencies, institutionserviceContext context, IInstitutionIncludedRepository institutionIncludedRepository)
        {
            _appSettings = appSettings.Value;
            _dependencies = dependencies.Value;
            _context = context;
            _institutionIncludedRepository = institutionIncludedRepository;
        }

        public dynamic InsertInstitutions(InstitutionsModel Model)
        {
            try
            {
                if (Model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                Institutions objInstitutions = new Institutions()
                {
                    Name = Model.Name,
                    CreatedAt = DateTime.UtcNow,
                    PhoneNumber = Model.PhoneNumber,
                    CountryIso = Model.CountryIso
                };
                _context.Institutions.Add(objInstitutions);
                _context.SaveChanges();

                if (Model.services != null)
                {
                    foreach (var item in Model.services)
                    {
                        int serviceIdDecrypted = Obfuscation.Decode(item);
                        var servicesDetails = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
                        if (servicesDetails != null)
                        {
                            ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                            {
                                InstitutionId = objInstitutions.InstitutionId,
                                ServiceId = serviceIdDecrypted
                            };
                            _context.ServicesInstitutions.Add(objServicesinstitutions);
                            _context.SaveChanges();
                        }
                    }
                }
                return ReturnResponse.SuccessResponse(CommonMessage.InstitutionInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateInstitution(InstitutionsModel Model)
        {
            try
            {
                int institutionIdDecrypted = Obfuscation.Decode(Model.InstitutionId);
                if (Model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var institution = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                institution.Name = Model.Name;
                institution.PhoneNumber = Model.PhoneNumber;
                institution.CountryIso = Model.CountryIso;
                _context.Institutions.Update(institution);
                _context.SaveChanges();

                if (Model.services != null)
                {
                    var servicesInstitutions = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionIdDecrypted).ToList();
                    if (servicesInstitutions != null && servicesInstitutions.Count > 0)
                    {
                        foreach (var item in servicesInstitutions)
                        {
                            _context.ServicesInstitutions.Remove(item);
                            _context.SaveChanges();
                        }
                    }
                    foreach (var item in Model.services)
                    {
                        int serviceIdDecrypted = Obfuscation.Decode(item);
                        var servicesDetails = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
                        if (servicesDetails != null)
                        {
                            ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                            {
                                InstitutionId = institutionIdDecrypted,
                                ServiceId = serviceIdDecrypted
                            };
                            _context.ServicesInstitutions.Add(objServicesinstitutions);
                            _context.SaveChanges();
                        }
                    }
                }

                return ReturnResponse.SuccessResponse(CommonMessage.InstitutionUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic DeleteInstitution(string id)
        {
            try
            {
                int institutionIdDecrypted = Obfuscation.Decode(id);
                var institution = _context.Institutions.Include(x => x.Officers).Include(x => x.ServicesInstitutions).Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                if (institution.Officers != null && institution.Officers.Count > 0)
                {
                    _context.Officers.RemoveRange(institution.Officers);
                    _context.SaveChanges();
                }

                if (institution.ServicesInstitutions != null && institution.ServicesInstitutions.Count > 0)
                {
                    _context.ServicesInstitutions.RemoveRange(institution.ServicesInstitutions);
                    _context.SaveChanges();
                }
                _context.Institutions.Remove(institution);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.InstitutionDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetInstitutions(string institutionId, string includeType, Pagination pageInfo)
        {
            try
            {
                int totalCount = 0;
                InstitutionGetResponse response = new InstitutionGetResponse();
                List<InstitutionsModel> objInstitutionsModelList = new List<InstitutionsModel>();
                if (string.IsNullOrEmpty(institutionId))
                {
                    var modelList = (from institution in _context.Institutions
                                     select new InstitutionsModel()
                                     {
                                         InstitutionId = Obfuscation.Encode(institution.InstitutionId),
                                         Name = institution.Name,
                                         CreatedAt = institution.CreatedAt,
                                         PhoneNumber = institution.PhoneNumber,
                                         CountryIso = institution.CountryIso,
                                         services = institution.ServicesInstitutions.Select(x => x.ServiceId.ToString()).ToList()
                                     }).AsEnumerable().OrderBy(a => a.InstitutionId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    foreach (var item in modelList)
                    {
                        InstitutionsModel model = new InstitutionsModel();
                        List<string> services = new List<string>();
                        model.InstitutionId = item.InstitutionId;
                        model.Name = item.Name;
                        model.CreatedAt = item.CreatedAt;
                        model.PhoneNumber = item.PhoneNumber;
                        model.CountryIso = item.CountryIso;
                        foreach (var item1 in item.services)
                        {
                            services.Add(Obfuscation.Encode(Convert.ToInt32(item1)));
                            model.services = services;
                        }
                        objInstitutionsModelList.Add(model);
                    }

                    totalCount = _context.Institutions.ToList().Count();
                }
                else
                {
                    int institutionIdDecrypted = Obfuscation.Decode(institutionId);
                    var modelList = (from institution in _context.Institutions
                                     where institution.InstitutionId == institutionIdDecrypted
                                     select new InstitutionsModel()
                                     {
                                         InstitutionId = Obfuscation.Encode(institution.InstitutionId),
                                         Name = institution.Name,
                                         CreatedAt = institution.CreatedAt,
                                         PhoneNumber = institution.PhoneNumber,
                                         CountryIso = institution.CountryIso,
                                         services = institution.ServicesInstitutions.Select(x => x.ServiceId.ToString()).ToList()
                                     }).AsEnumerable().OrderBy(a => a.InstitutionId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();


                    foreach (var item in modelList)
                    {
                        InstitutionsModel model = new InstitutionsModel();
                        List<string> services = new List<string>();
                        model.InstitutionId = item.InstitutionId;
                        model.Name = item.Name;
                        model.CreatedAt = item.CreatedAt;
                        model.PhoneNumber = item.PhoneNumber;
                        model.CountryIso = item.CountryIso;
                        foreach (var item1 in item.services)
                        {
                            services.Add(Obfuscation.Encode(Convert.ToInt32(item1)));
                            model.services = services;
                        }
                        objInstitutionsModelList.Add(model);
                    }

                    totalCount = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).ToList().Count();
                }

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                dynamic includeData = new JObject();
                if (!string.IsNullOrEmpty(includeType))
                {
                    string[] includeArr = includeType.Split(',');
                    if (includeArr.Length > 0)
                    {
                        foreach (var item in includeArr)
                        {
                            if (item.ToLower() == "service" || item.ToLower() == "services")
                            {
                                includeData.services = _institutionIncludedRepository.GetServiceIncludedData(objInstitutionsModelList);
                            }
                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.status = true;
                response.message = CommonMessage.InstitutionRetrived;
                response.pagination = page;
                response.data = objInstitutionsModelList;
                response.included = includeData;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic GetInstitutionsOfficers(string institutionId, Pagination pageInfo)
        {
            OfficersGetResponse response = new OfficersGetResponse();
            int totalCount = 0;
            try
            {
                List<OfficersModel> officersModelList = new List<OfficersModel>();

                if (string.IsNullOrEmpty(institutionId))
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);
                else
                {
                    int institutionIdDecrypted = Obfuscation.Decode(institutionId);
                    officersModelList = (from officers in _context.Officers
                                                where officers.InstitutionId == institutionIdDecrypted
                                                select new OfficersModel()
                                                {
                                                    OfficerId = Obfuscation.Encode(officers.OfficerId),
                                                    UserId = Obfuscation.Encode(officers.UserId.GetValueOrDefault()),
                                                    InstitutionId = Obfuscation.Encode(officers.InstitutionId.GetValueOrDefault()),
                                                }).AsEnumerable().OrderBy(a => a.InstitutionId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Officers.Where(x => x.InstitutionId == institutionIdDecrypted).ToList().Count();
                }

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount,
                };

                response.status = true;
                response.message = CommonMessage.OfficerRetrived;
                response.pagination = page;
                response.data = officersModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetInstitutionsDevices(string institutionId, Pagination pageInfo)
        {
            if (string.IsNullOrEmpty(institutionId))
                throw new ArgumentNullException(CommonMessage.BadRequest);

            List<VehiclesDto> VehiclesDtosList = JsonConvert.DeserializeObject<VehicleGetResponse>(GetAPI(_dependencies.InstitutionVehiclesUrl.Replace("|id|", institutionId)).Content).data;

            List<DevicesDto> devicesDtosList = new List<DevicesDto>();
            VehiclesDtosList.ForEach(vehicle => {
                List<DevicesDto> vehicleDevicesList = JsonConvert.DeserializeObject<DevicesGetResponse>(GetAPI(_dependencies.VehicleDevicesUrl.Replace("|id|", vehicle.VehicleId)).Content).data;
                vehicleDevicesList.ForEach(device => {
                    devicesDtosList.Add(device);
                });
            });

            return new DevicesGetResponse
            {
                data = devicesDtosList,
                pagination = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = devicesDtosList.Count,
                }
            };
        }

        private dynamic GetAPI(string url, string query = "")
        {
            UriBuilder uriBuilder = new UriBuilder(_appSettings.Host + url);
            uriBuilder = AppendQueryToUrl(uriBuilder, query);
            var client = new RestClient(uriBuilder.Uri);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == 0)
                throw new HttpListenerException(400, CommonMessage.ConnectionFailure);

            if (!response.IsSuccessful)
                throw new HttpListenerException((int)response.StatusCode, response.Content);

            return response;
        }

        private UriBuilder AppendQueryToUrl(UriBuilder baseUri, string queryToAppend)
        {
            if (baseUri.Query != null && baseUri.Query.Length > 1)
                baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
            else
                baseUri.Query = queryToAppend;
            return baseUri;
        }
    }
}
