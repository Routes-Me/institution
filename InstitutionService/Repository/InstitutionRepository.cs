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
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly institutionserviceContext _context;
        public InstitutionRepository(institutionserviceContext context)
        {
            _context = context;
        }

        public InstitutionResponse InsertInstitutions(InstitutionsModel Model)
        {
            InstitutionResponse response = new InstitutionResponse();
            try
            {
                if (Model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }
                Institutions objInstitutions = new Institutions()
                {
                    Name = Model.Name,
                    CreatedAt = DateTime.UtcNow,
                    PhoneNumber = Model.PhoneNumber,
                    CountryIso = Model.CountryIso
                };
                _context.Institutions.Add(objInstitutions);
                _context.SaveChanges();

                foreach (var item in Model.services)
                {
                    var servicesDetails = _context.Services.Where(x => x.ServiceId == item).FirstOrDefault();
                    if (servicesDetails != null)
                    {
                        Servicesinstitutions objServicesinstitutions = new Servicesinstitutions()
                        {
                            InstitutionId = objInstitutions.InstitutionId,
                            ServiceId = item
                        };
                        _context.Servicesinstitutions.Add(objServicesinstitutions);
                        _context.SaveChanges();
                    }
                }

                response.status = true;
                response.message = "Institution inserted successfully.";
                response.responseCode = ResponseCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while inserting institution. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public InstitutionResponse UpdateInstitution(InstitutionsModel Model)
        {
            InstitutionResponse response = new InstitutionResponse();
            try
            {
                if (Model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var institutionsData = _context.Institutions.Where(x => x.InstitutionId == Model.InstitutionId).FirstOrDefault();
                if (institutionsData == null)
                {
                    response.status = false;
                    response.message = "Institutions does not exist.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                institutionsData.Name = Model.Name;
                institutionsData.CreatedAt = DateTime.UtcNow;
                institutionsData.PhoneNumber = Model.PhoneNumber;
                institutionsData.CountryIso = Model.CountryIso;

                _context.Institutions.Update(institutionsData);
                _context.SaveChanges();
                response.status = true;
                response.message = "Institution updated successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while updating institution. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public InstitutionResponse DeleteInstitution(int id)
        {
            InstitutionResponse response = new InstitutionResponse();
            try
            {
                var institutionData = _context.Institutions.Where(x => x.InstitutionId == id).FirstOrDefault();
                if (institutionData == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                }

                //var vehicleData = _context.Vehicles.Where(x => x.InstitutionId == id).FirstOrDefault();
                //if (vehicleData != null)
                //{
                //    response.status = false;
                //    response.message = "Institution associated with other vehicles.";
                //    response.institutionDetails = null;
                //    response.responseCode = ResponseCode.Conflict;
                //}
                //var driversData = _context.Drivers.Where(x => x.InstitutionId == id).FirstOrDefault();
                //if (driversData != null)
                //{
                //    response.status = false;
                //    response.message = "Institution associated with other drivers.";
                //    response.institutionDetails = null;
                //    response.responseCode = ResponseCode.Conflict;
                //}
                var officersData = _context.Officers.Where(x => x.InstitutionId == id).FirstOrDefault();
                if (officersData != null)
                {
                    _context.Officers.Remove(officersData);
                    _context.SaveChanges();
                }
                _context.Institutions.Remove(institutionData);
                _context.SaveChanges();
                response.status = true;
                response.message = "Institution deleted successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while deleting institution. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public InstitutionGetResponse GetInstitutions(int institutionId, PageInfo pageInfo)
        {
            InstitutionGetResponse response = new InstitutionGetResponse();
            InstitutionDetails institutionDetails = new InstitutionDetails();
            int totalCount = 0;
            try
            {
                List<GetInstitutionsModel> objInstitutionsModelList = new List<GetInstitutionsModel>();
                if (institutionId == 0)
                {
                    objInstitutionsModelList = (from institution in _context.Institutions
                                                select new GetInstitutionsModel()
                                                {
                                                    InstitutionId = institution.InstitutionId,
                                                    Name = institution.Name,
                                                    CreatedAt = institution.CreatedAt,
                                                    PhoneNumber = institution.PhoneNumber,
                                                    CountryIso = institution.CountryIso,
                                                }).OrderBy(a => a.InstitutionId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Institutions.ToList().Count();
                }
                else
                {
                    objInstitutionsModelList = (from institution in _context.Institutions
                                                where institution.InstitutionId == institutionId
                                                select new GetInstitutionsModel()
                                                {
                                                    InstitutionId = institution.InstitutionId,
                                                    Name = institution.Name,
                                                    CreatedAt = institution.CreatedAt,
                                                    PhoneNumber = institution.PhoneNumber,
                                                    CountryIso = institution.CountryIso,
                                                }).OrderBy(a => a.InstitutionId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Institutions.Where(x => x.InstitutionId == institutionId).ToList().Count();
                }
                if (objInstitutionsModelList == null || objInstitutionsModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                institutionDetails.institutions = objInstitutionsModelList;
                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Institutions data retrived successfully.";
                response.pagination = page;
                response.data = institutionDetails;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching institutions. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
