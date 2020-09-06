using InstitutionService.Abstraction;
using InstitutionService.Helper.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class ServicesInstitutionsRepository : IServicesInstitutionsRepository
    {
        private readonly institutionserviceContext _context;
        private readonly  IServiceInstitutionIncludedRepository _serviceInstitutionIncludedRepository;
        public ServicesInstitutionsRepository(institutionserviceContext context, IServiceInstitutionIncludedRepository serviceInstitutionIncludedRepository)
        {
            _context = context;
            _serviceInstitutionIncludedRepository = serviceInstitutionIncludedRepository;
        }

        public ServicesInstitutionsResponse DeleteServicesInstitutions(int institutionId, int serviceId)
        {
            ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
            try
            {
                if (institutionId == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionId).FirstOrDefault();
                if (InstitutionDetails == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                if (serviceId <= 0)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var servicesinstitutions = _context.ServicesInstitutions.Where(x => x.InstitutionId == institutionId && x.ServiceId == serviceId).FirstOrDefault();
                if (servicesinstitutions == null)
                {
                    response.status = false;
                    response.message = "Services institutions not found.";
                    response.responseCode = ResponseCode.NotFound;
                }

                _context.ServicesInstitutions.Remove(servicesinstitutions);
                _context.SaveChanges();
                response.status = true;
                response.message = "Services institutions deleted successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while deleting services institutions. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesInstitutionsGetResponse GetServicesInstitutions(int institutionId, int serviceId, string includeType, Pagination pageInfo)
        {
            ServicesInstitutionsGetResponse response = new ServicesInstitutionsGetResponse();
            int totalCount = 0;
            try
            {
                if (institutionId == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionId).FirstOrDefault();
                if (InstitutionDetails == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                if (serviceId < 0)
                {
                    response.status = false;
                    response.message = "Pass valid serviceId.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

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
                {
                    response.status = false;
                    response.message = "Services institutions not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
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
                            else if(item.ToLower() == "service" || item.ToLower() == "services")
                            {
                                includeData.services = _serviceInstitutionIncludedRepository.GetServiceIncludedData(objServicesInstitutionsModel);
                            }
                             
                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.status = true;
                response.message = "Services institutions data retrived successfully.";
                response.pagination = page;
                response.data = objServicesInstitutionsModel;
                response.included = includeData;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching data. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesInstitutionsResponse InsertServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel model)
        {
            ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
            try
            {
                if (institutionsId == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionsId).FirstOrDefault();
                if (InstitutionDetails == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                if (model == null && model.ServiceId <= 0)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                ServicesInstitutions objServicesinstitutions = new ServicesInstitutions()
                {
                    InstitutionId = institutionsId,
                    ServiceId = model.ServiceId
                };
                _context.ServicesInstitutions.Add(objServicesinstitutions);
                _context.SaveChanges();
                response.status = true;
                response.message = "Services institutions inserted successfully.";
                response.responseCode = ResponseCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while inserting services institutions. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesInstitutionsResponse UpdateServicesInstitutions(int institutionsId, ServicesInstitutionsPostModel model)
        {
            ServicesInstitutionsResponse response = new ServicesInstitutionsResponse();
            try
            {
                //if (model == null)
                //{
                //    response.status = false;
                //    response.message = "Pass valid data in model.";
                //    response.responseCode = ResponseCode.BadRequest;
                //    return response;
                //}

                //var servicesinstitutions = _context.ServicesInstitutions.Where(x => x.Id == model.id).FirstOrDefault();
                //if (servicesinstitutions == null)
                //{
                //    response.status = false;
                //    response.message = "Services institutions not found.";
                //    response.responseCode = ResponseCode.NotFound;
                //    return response;
                //}

                //servicesinstitutions.ServiceId = model.ServiceId;
                //servicesinstitutions.InstitutionId = institutionsId;
                //_context.ServicesInstitutions.Update(servicesinstitutions);
                //_context.SaveChanges();

                response.status = true;
                response.message = "Services institutions updated successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while updating services institutions. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
