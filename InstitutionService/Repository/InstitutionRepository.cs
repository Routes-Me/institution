using InstitutionService.Abstraction;
using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Obfuscation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace InstitutionService.Repository
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IInstitutionIncludedRepository _institutionIncludedRepository;
        private readonly AppSettings _appSettings;

        public InstitutionRepository(IOptions<AppSettings> appSettings, institutionserviceContext context, IInstitutionIncludedRepository institutionIncludedRepository)
        {
            _appSettings = appSettings.Value;
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
                        int serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item), _appSettings.PrimeInverse);
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
                int institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(Model.InstitutionId), _appSettings.PrimeInverse);
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
                        int serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(item), _appSettings.PrimeInverse);
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
                int institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(id), _appSettings.PrimeInverse);
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
                int institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(institutionId), _appSettings.PrimeInverse);
                int totalCount = 0;
                InstitutionGetResponse response = new InstitutionGetResponse();
                List<InstitutionsModel> objInstitutionsModelList = new List<InstitutionsModel>();
                if (institutionIdDecrypted == 0)
                {
                    var modelList = (from institution in _context.Institutions
                                     select new InstitutionsModel()
                                     {
                                         InstitutionId = ObfuscationClass.EncodeId(institution.InstitutionId, _appSettings.Prime).ToString(),
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
                            services.Add(ObfuscationClass.EncodeId(Convert.ToInt32(item1), _appSettings.Prime).ToString());
                            model.services = services;
                        }
                        objInstitutionsModelList.Add(model);
                    }

                    totalCount = _context.Institutions.ToList().Count();
                }
                else
                {
                    var modelList = (from institution in _context.Institutions
                                     where institution.InstitutionId == institutionIdDecrypted
                                     select new InstitutionsModel()
                                     {
                                         InstitutionId = ObfuscationClass.EncodeId(institution.InstitutionId, _appSettings.Prime).ToString(),
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
                            services.Add(ObfuscationClass.EncodeId(Convert.ToInt32(item1), _appSettings.Prime).ToString());
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
    }
}
