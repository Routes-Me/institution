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
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IInstitutionIncludedRepository _institutionIncludedRepository;
        public InstitutionRepository(institutionserviceContext context, IInstitutionIncludedRepository institutionIncludedRepository)
        {
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
                        var servicesDetails = _context.Services.Where(x => x.ServiceId == item).FirstOrDefault();
                        if (servicesDetails != null)
                        {
                            ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                            {
                                InstitutionId = objInstitutions.InstitutionId,
                                ServiceId = item
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
                if (Model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var institution = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(Model.InstitutionId)).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                institution.Name = Model.Name;
                institution.CreatedAt = DateTime.UtcNow;
                institution.PhoneNumber = Model.PhoneNumber;
                institution.CountryIso = Model.CountryIso;
                _context.Institutions.Update(institution);
                _context.SaveChanges();
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
                var institution = _context.Institutions.Include(x => x.Officers).Include(x => x.ServicesInstitutions).Where(x => x.InstitutionId == Convert.ToInt32(id)).FirstOrDefault();
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
                List<GetInstitutionsModel> objInstitutionsModelList = new List<GetInstitutionsModel>();
                if (institutionId == "0")
                {
                    objInstitutionsModelList = (from institution in _context.Institutions
                                                select new GetInstitutionsModel()
                                                {
                                                    InstitutionId = institution.InstitutionId.ToString(),
                                                    Name = institution.Name,
                                                    CreatedAt = institution.CreatedAt,
                                                    PhoneNumber = institution.PhoneNumber,
                                                    CountryIso = institution.CountryIso,
                                                }).OrderBy(a => a.InstitutionId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = (from institution in _context.Institutions
                                  select new GetInstitutionsModel()
                                  {
                                      InstitutionId = institution.InstitutionId.ToString(),
                                      Name = institution.Name,
                                      CreatedAt = institution.CreatedAt,
                                      PhoneNumber = institution.PhoneNumber,
                                      CountryIso = institution.CountryIso,
                                  }).ToList().Count();
                }
                else
                {
                    objInstitutionsModelList = (from institution in _context.Institutions
                                                where institution.InstitutionId == Convert.ToInt32(institutionId)
                                                select new GetInstitutionsModel()
                                                {
                                                    InstitutionId = institution.InstitutionId.ToString(),
                                                    Name = institution.Name,
                                                    CreatedAt = institution.CreatedAt,
                                                    PhoneNumber = institution.PhoneNumber,
                                                    CountryIso = institution.CountryIso,
                                                }).OrderBy(a => a.InstitutionId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(institutionId)).ToList().Count();
                }
                if (objInstitutionsModelList == null || objInstitutionsModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

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
