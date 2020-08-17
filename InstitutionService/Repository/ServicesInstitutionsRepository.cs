using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Repository
{
    public class ServicesInstitutionsRepository : IServicesInstitutionsRepository
    {
        private readonly institutionserviceContext _context;
        public ServicesInstitutionsRepository(institutionserviceContext context)
        {
            _context = context;
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

                var servicesinstitutions = _context.Servicesinstitutions.Where(x => x.InstitutionId == institutionId && x.ServiceId == serviceId).FirstOrDefault();
                if (servicesinstitutions == null)
                {
                    response.status = false;
                    response.message = "Services institutions not found.";
                    response.responseCode = ResponseCode.NotFound;
                }

                _context.Servicesinstitutions.Remove(servicesinstitutions);
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

        public ServicesInstitutionsGetResponse GetServicesInstitutions(int institutionId, int serviceId, PageInfo pageInfo)
        {
            ServicesInstitutionsGetResponse response = new ServicesInstitutionsGetResponse();
            ServicesInstitutionsDetails servicesInstitutionsDetails = new ServicesInstitutionsDetails();
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
                    objServicesInstitutionsModel = (from servicesinstitution in _context.Servicesinstitutions
                                                    where servicesinstitution.InstitutionId == institutionId
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = servicesinstitution.InstitutionId,
                                                        ServiceId = servicesinstitution.ServiceId
                                                    }).OrderBy(a => a.ServiceId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Servicesinstitutions.Where(x => x.InstitutionId == institutionId).ToList().Count();
                }
                else
                {
                    objServicesInstitutionsModel = (from servicesinstitution in _context.Servicesinstitutions
                                                    where servicesinstitution.InstitutionId == institutionId && servicesinstitution.ServiceId == serviceId
                                                    select new ServicesInstitutionsModel()
                                                    {
                                                        InstitutionId = servicesinstitution.InstitutionId,
                                                        ServiceId = servicesinstitution.ServiceId
                                                    }).OrderBy(a => a.ServiceId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Servicesinstitutions.Where(x => x.InstitutionId == institutionId && x.ServiceId == serviceId).ToList().Count();
                }

                if (objServicesInstitutionsModel == null || objServicesInstitutionsModel.Count == 0)
                {
                    response.status = false;
                    response.message = "Services institutions not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                servicesInstitutionsDetails.servicesInstitutions = objServicesInstitutionsModel;
                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Services institutions data retrived successfully.";
                response.pagination = page;
                response.data = servicesInstitutionsDetails;
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

                Servicesinstitutions objServicesinstitutions = new Servicesinstitutions()
                {
                    InstitutionId = institutionsId,
                    ServiceId = model.ServiceId
                };
                _context.Servicesinstitutions.Add(objServicesinstitutions);
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
                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var servicesinstitutions = _context.Servicesinstitutions.Where(x => x.Id == model.id).FirstOrDefault();
                if (servicesinstitutions == null)
                {
                    response.status = false;
                    response.message = "Services institutions not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                servicesinstitutions.ServiceId = model.ServiceId;
                servicesinstitutions.InstitutionId = institutionsId;
                _context.Servicesinstitutions.Update(servicesinstitutions);
                _context.SaveChanges();

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
