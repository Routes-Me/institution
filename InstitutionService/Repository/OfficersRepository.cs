using InstitutionService.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using InstitutionService.Models.DBModels;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using Newtonsoft.Json.Linq;
using InstitutionService.Helper.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using InstitutionService.Helper.Models;
using Microsoft.Extensions.Options;
using RoutesSecurity;

namespace InstitutionService.Repository
{
    public class OfficersRepository : IOfficersRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IOfficersIncludedRepository _officersIncludedRepository;
        private readonly AppSettings _appSettings;

        public OfficersRepository(IOptions<AppSettings> appSettings, institutionserviceContext context, IOfficersIncludedRepository officersIncludedRepository)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _officersIncludedRepository = officersIncludedRepository;
        }

        public dynamic DeleteOfficers(string officerId)
        {
            try
            {
                int officerIdDecrypted = Obfuscation.Decode(officerId);
                var officers = _context.Officers.Include(x => x.Invitations).Where(x => x.OfficerId == officerIdDecrypted).FirstOrDefault();
                if (officers == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                if (officers.Invitations != null)
                {
                    _context.Invitations.RemoveRange(officers.Invitations);
                    _context.SaveChanges();
                }
                _context.Officers.Remove(officers);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.OfficerDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetOfficers(string officerId, string userId, string includeType, Pagination pageInfo)
        {
            try
            {
                int totalCount = 0;
                OfficersGetResponse response = new OfficersGetResponse();
                List<OfficersModel> objOfficersModelList = new List<OfficersModel>();
                if (string.IsNullOrEmpty(officerId))
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        objOfficersModelList = (from officer in _context.Officers
                                                select new OfficersModel()
                                                {
                                                    OfficerId = officer.OfficerId.ToString(),
                                                    InstitutionId = officer.InstitutionId.ToString(),
                                                    UserId = officer.UserId.ToString(),
                                                }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                        totalCount = _context.Officers.ToList().Count();
                    }
                    else
                    {
                        int userIdDecrypted = Obfuscation.Decode(userId);
                        objOfficersModelList = (from officer in _context.Officers
                                                where officer.UserId == Convert.ToInt32(userIdDecrypted)
                                                select new OfficersModel()
                                                {
                                                    OfficerId = officer.OfficerId.ToString(),
                                                    InstitutionId = officer.InstitutionId.ToString(),
                                                    UserId = officer.UserId.ToString(),
                                                }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                        totalCount = _context.Officers.Where(x => x.UserId == Convert.ToInt32(userIdDecrypted)).ToList().Count();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        int officerIdDecrypted = Obfuscation.Decode(officerId);
                        objOfficersModelList = (from officer in _context.Officers
                                                where officer.OfficerId == Convert.ToInt32(officerIdDecrypted)
                                                select new OfficersModel()
                                                {
                                                    OfficerId = officer.OfficerId.ToString(),
                                                    InstitutionId = officer.InstitutionId.ToString(),
                                                    UserId = officer.UserId.ToString(),
                                                }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                        totalCount = _context.Officers.Where(x => x.OfficerId == Convert.ToInt32(officerIdDecrypted)).ToList().Count();
                    }
                    else
                    {
                        int officerIdDecrypted = Obfuscation.Decode(officerId);
                        int userIdDecrypted = Obfuscation.Decode(userId);
                        objOfficersModelList = (from officer in _context.Officers
                                                where officer.OfficerId == Convert.ToInt32(officerIdDecrypted)
                                                && officer.UserId == Convert.ToInt32(userIdDecrypted)
                                                select new OfficersModel()
                                                {
                                                    OfficerId = officer.OfficerId.ToString(),
                                                    InstitutionId = officer.InstitutionId.ToString(),
                                                    UserId = officer.UserId.ToString(),
                                                }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                        totalCount = _context.Officers.Where(x => x.OfficerId == Convert.ToInt32(officerIdDecrypted) && x.UserId == Convert.ToInt32(userIdDecrypted)).ToList().Count();
                    }
                }
                List<OfficersModel> objOfficersModelListNew = new List<OfficersModel>();
                if (objOfficersModelList.Count > 0)
                {
                    foreach (var item in objOfficersModelList)
                    {
                        OfficersModel obj = new OfficersModel();
                        obj.OfficerId = Obfuscation.Encode(Convert.ToInt32(item.OfficerId));
                        obj.InstitutionId = Obfuscation.Encode(Convert.ToInt32(item.InstitutionId));
                        obj.UserId = Obfuscation.Encode(Convert.ToInt32(item.UserId));
                        objOfficersModelListNew.Add(obj);
                    }
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
                            if (item.ToLower() == "user" || item.ToLower() == "users")
                            {
                                includeData.users = _officersIncludedRepository.GetUsersIncludedData(objOfficersModelListNew);
                            }
                            else if (item.ToLower() == "institution" || item.ToLower() == "institutions")
                            {
                                includeData.institutions = _officersIncludedRepository.GetInstitutionsIncludedData(objOfficersModelListNew);
                            }
                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.status = true;
                response.message = CommonMessage.OfficerRetrived;
                response.pagination = page;
                response.data = objOfficersModelListNew;
                response.included = includeData;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertOfficers(OfficersModel model)
        {
            try
            {
                int institutionIdDecrypted = Obfuscation.Decode(model.InstitutionId);
                int userIdDecrypted = Obfuscation.Decode(model.UserId);
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                Officers objOfficers = new Officers()
                {
                    UserId = userIdDecrypted,
                    InstitutionId = institutionIdDecrypted
                };
                _context.Officers.Add(objOfficers);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.OfficerInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateOfficers(OfficersModel model)
        {
            try
            {
                int officerIdDecrypted = Obfuscation.Decode(model.OfficerId);
                int institutionIdDecrypted = Obfuscation.Decode(model.InstitutionId);
                int userIdDecrypted = Obfuscation.Decode(model.UserId);
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var officers = _context.Officers.Include(x => x.Institution).Where(x => x.OfficerId == officerIdDecrypted).FirstOrDefault();
                if (officers == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                officers.InstitutionId = institutionIdDecrypted;
                officers.UserId = userIdDecrypted;
                _context.Officers.Update(officers);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.OfficerUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
