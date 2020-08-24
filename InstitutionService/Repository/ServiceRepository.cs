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
    public class ServiceRepository : IServiceRepository
    {
        private readonly institutionserviceContext _context;
        public ServiceRepository(institutionserviceContext context)
        {
            _context = context;
        }
        public ServicesResponse DeleteService(int id)
        {
            ServicesResponse response = new ServicesResponse();
            try
            {
                var servicesData = _context.Services.Where(x => x.ServiceId == id).FirstOrDefault();
                if (servicesData == null)
                {
                    response.status = false;
                    response.message = "Service not found.";
                    response.responseCode = ResponseCode.NotFound;
                }
                var servicesInstitutions = _context.ServicesInstitutions.Where(x => x.ServiceId == id).ToList();
                if (servicesInstitutions != null)
                {
                    foreach (var item in servicesInstitutions)
                    {
                        _context.ServicesInstitutions.Remove(item);
                        _context.SaveChanges();
                    }
                }
                _context.Services.Remove(servicesData);
                _context.SaveChanges();
                response.status = true;
                response.message = "Service deleted successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while deleting service. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesGetResponse GetServices(int servicesId, Pagination pageInfo)
        {
            ServicesGetResponse response = new ServicesGetResponse();
            int totalCount = 0;
            try
            {
                List<ServicesModel> objServicesModelList = new List<ServicesModel>();
                if (servicesId == 0)
                {
                    objServicesModelList = (from services in _context.Services
                                            select new ServicesModel()
                                            {
                                                ServiceId = services.ServiceId,
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.ToList().Count();
                }
                else
                {
                    objServicesModelList = (from services in _context.Services
                                            where services.ServiceId == servicesId
                                            select new ServicesModel()
                                            {
                                                ServiceId = services.ServiceId,
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.Where(x => x.ServiceId == servicesId).ToList().Count();
                }
                if (objServicesModelList == null || objServicesModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Services not found.";
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
                response.status = true;
                response.message = "Services data retrived successfully.";
                response.pagination = page;
                response.data = objServicesModelList;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching services. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesResponse InsertService(ServicesModel model)
        {
            ServicesResponse response = new ServicesResponse();
            try
            {
                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }
                Services objServices = new Services()
                {
                    Name = model.Name,
                    Descriptions = model.Description,
                };
                _context.Services.Add(objServices);
                _context.SaveChanges();
                response.status = true;
                response.message = "Service inserted successfully.";
                response.responseCode = ResponseCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while inserting service. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public ServicesResponse UpdateService(ServicesModel model)
        {
            ServicesResponse response = new ServicesResponse();
            try
            {
                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var servicesData = _context.Services.Where(x => x.ServiceId == model.ServiceId).FirstOrDefault();
                if (servicesData == null)
                {
                    response.status = false;
                    response.message = "Service does not exist.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                servicesData.Name = model.Name;
                servicesData.Descriptions = model.Description;
                _context.Services.Update(servicesData);
                _context.SaveChanges();
                response.status = true;
                response.message = "Service updated successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while updating service. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
