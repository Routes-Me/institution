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

namespace InstitutionService.Repository
{
    public class OfficersRepository : IOfficersRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IOfficersIncludedRepository _officersIncludedRepository;
        public OfficersRepository(institutionserviceContext context, IOfficersIncludedRepository officersIncludedRepository)
        {
            _context = context;
            _officersIncludedRepository = officersIncludedRepository;
        }

        public dynamic DeleteOfficers(string officerId)
        {
            try
            {
                var officers = _context.Officers.Include(x => x.Invitations).Where(x => x.OfficerId == Convert.ToInt32(officerId)).FirstOrDefault();
                if (officers == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                if(officers.Invitations != null)
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

        public dynamic GetOfficers(string officerId, string includeType, Pagination pageInfo)
        {
            try
            {
                int totalCount = 0;
                OfficersGetResponse response = new OfficersGetResponse();
                List<OfficersModel> objOfficersModelList = new List<OfficersModel>();
                if (officerId == "0")
                {
                    objOfficersModelList = (from officer in _context.Officers
                                            select new OfficersModel()
                                            {
                                                OfficerId = officer.OfficerId.ToString(),
                                                InstitutionId = officer.InstitutionId.ToString(),
                                                UserId = officer.UserId.ToString()
                                            }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Officers.ToList().Count();
                }
                else
                {
                    objOfficersModelList = (from officer in _context.Officers
                                            where officer.OfficerId == Convert.ToInt32(officerId)
                                            select new OfficersModel()
                                            {
                                                OfficerId = officer.OfficerId.ToString(),
                                                InstitutionId = officer.InstitutionId.ToString(),
                                                UserId = officer.UserId.ToString()
                                            }).OrderBy(a => a.OfficerId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Officers.Where(x => x.OfficerId == Convert.ToInt32(officerId)).ToList().Count();
                }

                if (objOfficersModelList == null || objOfficersModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

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
                                includeData.users = _officersIncludedRepository.GetUsersIncludedData(objOfficersModelList);
                            }
                            else if (item.ToLower() == "institution" || item.ToLower() == "institutions")
                            {
                                includeData.institutions = _officersIncludedRepository.GetInstitutionsIncludedData(objOfficersModelList);
                            }
                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.status = true;
                response.message = CommonMessage.OfficerRetrived;
                response.pagination = page;
                response.data = objOfficersModelList;
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
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(model.InstitutionId)).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                Officers objOfficers = new Officers()
                {
                    UserId = Convert.ToInt32(model.UserId),
                    InstitutionId = Convert.ToInt32(model.InstitutionId)
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
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var officers = _context.Officers.Include(x => x.Institution).Where(x => x.OfficerId == Convert.ToInt32(model.OfficerId)).FirstOrDefault();
                if (officers == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(model.InstitutionId)).FirstOrDefault();
                if (InstitutionDetails == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                officers.InstitutionId = Convert.ToInt32(model.InstitutionId);
                officers.UserId = Convert.ToInt32(model.UserId);
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
