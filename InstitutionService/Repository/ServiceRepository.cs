using InstitutionService.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Obfuscation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly institutionserviceContext _context;
        private readonly AppSettings _appSettings;

        public ServiceRepository(IOptions<AppSettings> appSettings, institutionserviceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public dynamic DeleteService(string id)
        {
            try
            {
                int serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(id), _appSettings.PrimeInverse);
                var service = _context.Services.Include(x => x.ServicesInstitutions).Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
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
                int serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(servicesId), _appSettings.PrimeInverse);
                int totalCount = 0;
                ServicesGetResponse response = new ServicesGetResponse();
                List<ServicesModel> objServicesModelList = new List<ServicesModel>();
                if (serviceIdDecrypted == 0)
                {
                    objServicesModelList = (from services in _context.Services
                                            select new ServicesModel()
                                            {
                                                ServiceId = ObfuscationClass.EncodeId(services.ServiceId, _appSettings.Prime).ToString(),
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).AsEnumerable().OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.ToList().Count();
                }
                else
                {
                    objServicesModelList = (from services in _context.Services
                                            where services.ServiceId == serviceIdDecrypted
                                            select new ServicesModel()
                                            {
                                                ServiceId = ObfuscationClass.EncodeId(services.ServiceId, _appSettings.Prime).ToString(),
                                                Name = services.Name,
                                                Description = services.Descriptions,
                                            }).AsEnumerable().OrderBy(a => a.ServiceId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).ToList().Count();
                }
               
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
                int serviceIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(model.ServiceId), _appSettings.PrimeInverse);
                var servicesData = _context.Services.Where(x => x.ServiceId == serviceIdDecrypted).FirstOrDefault();
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
