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
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class ServicesInstitutionsRepository : IServicesInstitutionsRepository
    {
        private readonly InstitutionsContext _context;
        private readonly IServiceInstitutionIncludedRepository _serviceInstitutionIncludedRepository;
        private readonly AppSettings _appSettings;

        public ServicesInstitutionsRepository(IOptions<AppSettings> appSettings, InstitutionsContext context, IServiceInstitutionIncludedRepository serviceInstitutionIncludedRepository)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _serviceInstitutionIncludedRepository = serviceInstitutionIncludedRepository;
        }

        public dynamic DeleteServicesInstitutions(string institutionId, string serviceId)
        {
            try
            {
                int serviceIdDecrypted = Obfuscation.Decode(serviceId);
                int institutionIdDecrypted = Obfuscation.Decode(institutionId);
                var institution = _context.Institutions.Include(x => x.ServicesInstitutions).Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                var servicesinstitutions = institution.ServicesInstitutions.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
                if (servicesinstitutions == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.ServiceInstitutionNotFound, StatusCodes.Status404NotFound);

                _context.ServicesInstitutions.Remove(servicesinstitutions);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.ServiceInstitutionDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic GetServicesInstitutions(string institutionId, string serviceId, string includeType, Pagination pageInfo)
        {
            try
            {
                int institutionIdDecrypted = Obfuscation.Decode(institutionId);
                int totalCount = 0;
                ServicesInstitutionsGetResponse response = new ServicesInstitutionsGetResponse();
                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                List<ServicesInstitutionsModel> objServicesInstitutionsModel = new List<ServicesInstitutionsModel>();
                if (string.IsNullOrEmpty(serviceId))
                {
                    objServicesInstitutionsModel = (from servicesinstitution in _context.ServicesInstitutions
                                                    where servicesinstitution.InstitutionId == institutionIdDecrypted
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = Obfuscation.Encode(servicesinstitution.InstitutionId),
                                                        ServiceId = Obfuscation.Encode(servicesinstitution.ServiceId)
                                                    }).AsEnumerable().OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionIdDecrypted).ToList().Count();
                }
                else
                {
                    int serviceIdDecrypted = Obfuscation.Decode(serviceId);
                    objServicesInstitutionsModel = (from servicesinstitution in _context.ServicesInstitutions
                                                    where servicesinstitution.InstitutionId == institutionIdDecrypted && servicesinstitution.ServiceId == serviceIdDecrypted
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = Obfuscation.Encode(servicesinstitution.InstitutionId),
                                                        ServiceId = Obfuscation.Encode(servicesinstitution.ServiceId)
                                                    }).AsEnumerable().OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionIdDecrypted && x.ServiceId == serviceIdDecrypted).ToList().Count();
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
                            if (item.ToLower() == "institution" || item.ToLower() == "institutions")
                            {
                                includeData.institutions = _serviceInstitutionIncludedRepository.GetInstitutionsIncludedData(objServicesInstitutionsModel);
                            }
                            else if (item.ToLower() == "service" || item.ToLower() == "services")
                            {
                                includeData.services = _serviceInstitutionIncludedRepository.GetServiceIncludedData(objServicesInstitutionsModel);
                            }
                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.status = true;
                response.message = CommonMessage.ServiceInstitutionRetrived;
                response.pagination = page;
                response.data = objServicesInstitutionsModel;
                response.included = includeData;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic InsertServicesInstitutions(string institutionId, ServicesInstitutionsPostModel model)
        {
            try
            {
                int serviceIdDecrypted = Obfuscation.Decode(model.ServiceId);
                int institutionIdDecrypted = Obfuscation.Decode(institutionId);
                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                if (model == null || Convert.ToInt32(model.ServiceId) <= 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                {
                    InstitutionId = institutionIdDecrypted,
                    ServiceId = serviceIdDecrypted
                };
                _context.ServicesInstitutions.Add(objServicesinstitutions);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.ServiceInstitutionInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
