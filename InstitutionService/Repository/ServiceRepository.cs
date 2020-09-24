using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly institutionserviceContext _context;
        public ServiceRepository(institutionserviceContext context)
        {
            _context = context;
        }
        public dynamic DeleteService(string id)
        {
            try
            {
                var service = _context.Services.Include(x => x.ServicesInstitutions).Where(x => x.ServiceId == Convert.ToInt32(id)).FirstOrDefault();
                if (service == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.ServiceNotFound, StatusCodes.Status404NotFound);
                
                if (service.ServicesInstitutions != null && service.ServicesInstitutions.Count > 0)
                {
                    _context.ServicesInstitutions.RemoveRange(service.ServicesInstitutions);
                    _context.SaveChanges();
                }
                _context.Services.Remove(service);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.ServiceDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic GetServices(string servicesId, Pagination pageInfo)
        {
            try
            {
                int totalCount = 0;
                ServicesGetResponse response = new ServicesGetResponse();
                List<ServicesModel> objServicesModelList = new List<ServicesModel>();
                if (servicesId == "0")
                {
                    objServicesModelList = (from services in _context.Services
                                            select new ServicesModel()
                                            {
                                                ServiceId = services.ServiceId.ToString(),
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.ToList().Count();
                }
                else
                {
                    objServicesModelList = (from services in _context.Services
                                            where services.ServiceId == Convert.ToInt32(servicesId)
                                            select new ServicesModel()
                                            {
                                                ServiceId = services.ServiceId.ToString(),
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.Where(x => x.ServiceId == Convert.ToInt32(servicesId)).ToList().Count();
                }
                if (objServicesModelList == null || objServicesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.ServiceNotFound, StatusCodes.Status404NotFound);
                
                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };
                response.status = true;
                response.message = CommonMessage.ServiceRetrived;
                response.pagination = page;
                response.data = objServicesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic InsertService(ServicesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);
                
                Services objServices = new Services()
                {
                    Name = model.Name,
                    Descriptions = model.Description,
                };
                _context.Services.Add(objServices);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.ServiceInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
        public dynamic UpdateService(ServicesModel model)
        {
            try
            {
                var servicesData = _context.Services.Where(x => x.ServiceId == Convert.ToInt32(model.ServiceId)).FirstOrDefault();
                if (servicesData == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.ServiceNotFound, StatusCodes.Status404NotFound);

                servicesData.Name = model.Name;
                servicesData.Descriptions = model.Description;
                _context.Services.Update(servicesData);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.ServiceUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
