using InstitutionService.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionService.Models.DBModels;
using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;

namespace InstitutionService.Repository
{
    public class OfficersRepository : IOfficersRepository
    {
        private readonly institutionserviceContext _context;
        public OfficersRepository(institutionserviceContext context)
        {
            _context = context;
        }

        public OfficersResponse DeleteOfficers(int officerId)
        {
            OfficersResponse response = new OfficersResponse();
            try
            {
                var officers = _context.Officers.Where(x => x.OfficerId == officerId).FirstOrDefault();
                if (officers == null)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.responseCode = ResponseCode.NotFound;
                }

                _context.Officers.Remove(officers);
                _context.SaveChanges();
                response.status = true;
                response.message = "Officer deleted successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while deleting officer. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public OfficersGetResponse GetOfficers(int officerId, PageInfo pageInfo)
        {
            OfficersGetResponse response = new OfficersGetResponse();
            OfficersDetails officersDetails = new OfficersDetails();
            int totalCount = 0;
            try
            {
                List<OfficersModel> objOfficersModelList = new List<OfficersModel>();
                if (officerId == 0)
                {
                    objOfficersModelList = (from officer in _context.Officers
                                            select new OfficersModel()
                                            {
                                                OfficerId = officer.OfficerId,
                                                InstitutionId = officer.InstitutionId,
                                                UserId = officer.UserId
                                            }).OrderBy(a => a.OfficerId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Officers.ToList().Count();
                }
                else
                {
                    objOfficersModelList = (from officer in _context.Officers
                                            where officer.OfficerId == officerId
                                            select new OfficersModel()
                                            {
                                                OfficerId = officer.OfficerId,
                                                InstitutionId = officer.InstitutionId,
                                                UserId = officer.UserId
                                            }).OrderBy(a => a.OfficerId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Officers.Where(x => x.OfficerId == officerId).ToList().Count();
                }

                if (objOfficersModelList == null || objOfficersModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                officersDetails.officers = objOfficersModelList;
                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Officers data retrived successfully.";
                response.pagination = page;
                response.data = officersDetails;
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

        public OfficersResponse InsertOfficers(OfficersModel model)
        {
            OfficersResponse response = new OfficersResponse();
            try
            {
                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == model.InstitutionId).FirstOrDefault();
                if (InstitutionDetails == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                Officers objOfficers = new Officers()
                {
                    UserId = model.UserId,
                    InstitutionId = model.InstitutionId
                };
                _context.Officers.Add(objOfficers);
                _context.SaveChanges();
                response.status = true;
                response.message = "Officers inserted successfully.";
                response.responseCode = ResponseCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while inserting officers. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public OfficersResponse UpdateOfficers(OfficersModel model)
        {
            OfficersResponse response = new OfficersResponse();
            try
            {
                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var officers = _context.Officers.Where(x => x.OfficerId == model.OfficerId).FirstOrDefault();
                if (officers == null)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                var InstitutionDetails = _context.Institutions.Where(x => x.InstitutionId == model.InstitutionId).FirstOrDefault();
                if (InstitutionDetails == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                officers.InstitutionId = model.InstitutionId;
                officers.UserId = model.UserId;
                _context.Officers.Update(officers);
                _context.SaveChanges();

                response.status = true;
                response.message = "Officer updated successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while updating officer. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
