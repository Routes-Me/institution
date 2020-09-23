using InstitutionService.Abstraction;
using InstitutionService.Helper.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class ServicesInstitutionsRepository : IServicesInstitutionsRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IServiceInstitutionIncludedRepository _serviceInstitutionIncludedRepository;
        public ServicesInstitutionsRepository(institutionserviceContext context, IServiceInstitutionIncludedRepository serviceInstitutionIncludedRepository)
        {
            _context = context;
            _serviceInstitutionIncludedRepository = serviceInstitutionIncludedRepository;
        }

        public dynamic DeleteServicesInstitutions(int institutionId, int serviceId)
        {
            try
            {
                var institution = _context.Institutions.Include(x => x.ServicesInstitutions).Where(x => x.InstitutionId == institutionId).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                var servicesinstitutions = institution.ServicesInstitutions.Where(x => x.ServiceId == serviceId).FirstOrDefault();
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
        public dynamic GetServicesInstitutions(int institutionId, int serviceId, string includeType, Pagination pageInfo)
        {
            try
            {
                int totalCount = 0;
                ServicesInstitutionsGetResponse response = new ServicesInstitutionsGetResponse();
                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionId).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                List<ServicesInstitutionsModel> objServicesInstitutionsModel = new List<ServicesInstitutionsModel>();
                if (serviceId == 0)
                {
                    objServicesInstitutionsModel = (from servicesinstitution in _context.ServicesInstitutions
                                                    where servicesinstitution.InstitutionId == institutionId
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = servicesinstitution.InstitutionId,
                                                        ServiceId = servicesinstitution.ServiceId
                                                    }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionId).ToList().Count();
                }
                else
                {
                    objServicesInstitutionsModel = (from servicesinstitution in _context.ServicesInstitutions
                                                    where servicesinstitution.InstitutionId == institutionId && servicesinstitution.ServiceId == serviceId
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = servicesinstitution.InstitutionId,
                                                        ServiceId = servicesinstitution.ServiceId
                                                    }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionId && x.ServiceId == serviceId).ToList().Count();
                }

                if (objServicesInstitutionsModel == null || objServicesInstitutionsModel.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.ServiceInstitutionNotFound, StatusCodes.Status404NotFound);

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
        public dynamic InsertServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel model)
        {
            try
            {
                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionsId).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                if (model == null || model.ServiceId <= 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                {
                    InstitutionId = institutionsId,
                    ServiceId = model.ServiceId
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
