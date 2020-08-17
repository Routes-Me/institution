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
                    CreatedAt = Model.CreatedAt,
                    PhoneNumber = Model.PhoneNumber,
                    CountryIso = Model.CountryIso
                };
                _context.Institutions.Add(objInstitutions);
                _context.SaveChanges();
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
                institutionsData.CreatedAt = Model.CreatedAt;
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
                List<InstitutionsModel> objInstitutionsModelList = new List<InstitutionsModel>();
                if (institutionId == 0)
                {
                    objInstitutionsModelList = (from institution in _context.Institutions
                                                select new InstitutionsModel()
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
                                                select new InstitutionsModel()
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
                institutionDetails.institution = objInstitutionsModelList;
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

        public InstitutionVehicleResponse GetVehicles(int institutionId, int vehicleId, PageInfo pageInfo)
        {
            InstitutionVehicleResponse response = new InstitutionVehicleResponse();
            InstitutionVehicleDetails vehicleDeails = new InstitutionVehicleDetails();
            int totalCount = 0;
            try
            {
                List<VehiclesModel> objVehiclesModelList = new List<VehiclesModel>();
                if (institutionId == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                else
                {
                    var institutionCount = _context.Institutions.Where(x => x.InstitutionId == institutionId).ToList().Count();
                    if (institutionCount == 0)
                    {
                        response.status = false;
                        response.message = "Institution not found.";
                        response.data = null;
                        response.responseCode = ResponseCode.NotFound;
                        return response;
                    }
                    else
                    {
                        //if (vehicleId == 0)
                        //{
                        //    objVehiclesModelList = (from institution in _context.Institutions
                        //                            join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                            where institution.InstitutionId == institutionId
                        //                            select new VehiclesModel()
                        //                            {
                        //                                VehicleId = vehicle.VehicleId,
                        //                                DeviceId = vehicle.DeviceId,
                        //                                PlateNumber = vehicle.PlateNumber,
                        //                                InstitutionId = vehicle.InstitutionId,
                        //                                ModelYear = Convert.ToDateTime(vehicle.ModelYear).Year,
                        //                                ModelId = vehicle.ModelId
                        //                            }).OrderBy(a => a.VehicleId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                        //    totalCount = (from institution in _context.Institutions
                        //                         join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                         where institution.InstitutionId == institutionId
                        //                         select new VehiclesModel()
                        //                         {
                        //                             VehicleId = vehicle.VehicleId,
                        //                             DeviceId = vehicle.DeviceId,
                        //                             PlateNumber = vehicle.PlateNumber,
                        //                             InstitutionId = vehicle.InstitutionId,
                        //                             ModelYear = Convert.ToDateTime(vehicle.ModelYear).Year,
                        //                             ModelId = vehicle.ModelId
                        //                         }).ToList().Count;
                        //}
                        //else
                        //{

                        //    objVehiclesModelList = (from institution in _context.Institutions
                        //                            join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                            where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId
                        //                            select new VehiclesModel()
                        //                            {
                        //                                VehicleId = vehicle.VehicleId,
                        //                                DeviceId = vehicle.DeviceId,
                        //                                PlateNumber = vehicle.PlateNumber,
                        //                                InstitutionId = vehicle.InstitutionId,
                        //                                ModelYear = Convert.ToDateTime(vehicle.ModelYear).Year,
                        //                                ModelId = vehicle.ModelId
                        //                            }).OrderBy(a => a.VehicleId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                        //    totalCount = (from institution in _context.Institutions
                        //                         join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                         where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId
                        //                         select new VehiclesModel()
                        //                         {
                        //                             VehicleId = vehicle.VehicleId,
                        //                             DeviceId = vehicle.DeviceId,
                        //                             PlateNumber = vehicle.PlateNumber,
                        //                             InstitutionId = vehicle.InstitutionId,
                        //                             ModelYear = Convert.ToDateTime(vehicle.ModelYear).Year,
                        //                             ModelId = vehicle.ModelId
                        //                         }).ToList().Count;

                        //}
                    }
                }

                if (objVehiclesModelList == null || objVehiclesModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Vehicle not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                vehicleDeails.vehicles = objVehiclesModelList;

                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Vehicles data retrived successfully.";
                response.pagination = page;
                response.data = vehicleDeails;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching vehicles. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public InstitutionDriverResponse GetDrivers(int institutionId, int vehicleId, int driverId, PageInfo pageInfo)
        {
            InstitutionDriverResponse response = new InstitutionDriverResponse();
            InstitutionDriverDetails driverDeails = new InstitutionDriverDetails();
            int totalCount = 0;
            try
            {
                List<DriversModel> objDriversModelList = new List<DriversModel>();
                if (institutionId == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                else
                {
                    var institutionCount = _context.Institutions.Where(x => x.InstitutionId == institutionId).ToList().Count();
                    if (institutionCount == 0)
                    {
                        response.status = false;
                        response.message = "Institution not found.";
                        response.data = null;
                        response.responseCode = ResponseCode.NotFound;
                        return response;
                    }
                    else
                    {
                        if (vehicleId == 0)
                        {
                            response.status = false;
                            response.message = "Vehicle not found.";
                            response.data = null;
                            response.responseCode = ResponseCode.NotFound;
                            return response;
                        }
                        //else
                        //{
                        //    var vehicleCount = _context.Vehicles.Where(x => x.VehicleId == vehicleId).ToList().Count();
                        //    if (vehicleCount == 0)
                        //    {
                        //        response.status = false;
                        //        response.message = "Vehicle not found.";
                        //        response.data = null;
                        //        response.responseCode = ResponseCode.NotFound;
                        //        return response;
                        //    }
                        //    else
                        //    {
                        //        if (driverId == 0)
                        //        {
                        //            objDriversModelList = (from institution in _context.Institutions
                        //                                   join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                                   join drive in _context.Drives on vehicle.VehicleId equals drive.VehicleId
                        //                                   join driver in _context.Drivers on drive.DriverId equals driver.DriverId
                        //                                   where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId
                        //                                   select new DriversModel()
                        //                                   {
                        //                                       DriverId = driver.DriverId,
                        //                                       UserId = driver.UserId
                        //                                   }).OrderBy(a => a.DriverId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();


                        //            totalCount = (from institution in _context.Institutions
                        //                          join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                          join drive in _context.Drives on vehicle.VehicleId equals drive.VehicleId
                        //                          join driver in _context.Drivers on drive.DriverId equals driver.DriverId
                        //                          where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId
                        //                          select new DriversModel()
                        //                          {
                        //                              DriverId = driver.DriverId,
                        //                              UserId = driver.UserId
                        //                          }).ToList().Count();
                        //        }
                        //        else
                        //        {
                        //            objDriversModelList = (from institution in _context.Institutions
                        //                                   join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                                   join drive in _context.Drives on vehicle.VehicleId equals drive.VehicleId
                        //                                   join driver in _context.Drivers on drive.DriverId equals driver.DriverId
                        //                                   where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId && driver.DriverId == driverId
                        //                                   select new DriversModel()
                        //                                   {
                        //                                       DriverId = driver.DriverId,
                        //                                       UserId = driver.UserId
                        //                                   }).OrderBy(a => a.DriverId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();


                        //            totalCount = (from institution in _context.Institutions
                        //                          join vehicle in _context.Vehicles on institution.InstitutionId equals vehicle.InstitutionId
                        //                          join drive in _context.Drives on vehicle.VehicleId equals drive.VehicleId
                        //                          join driver in _context.Drivers on drive.DriverId equals driver.DriverId
                        //                          where institution.InstitutionId == institutionId && vehicle.VehicleId == vehicleId && driver.DriverId == driverId
                        //                          select new DriversModel()
                        //                          {
                        //                              DriverId = driver.DriverId,
                        //                              UserId = driver.UserId
                        //                          }).ToList().Count();
                        //        }
                        //    }
                        //}
                    }
                }

                if (objDriversModelList == null || objDriversModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Driver not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                driverDeails.drivers = objDriversModelList;
                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Drivers data retrived successfully.";
                response.pagination = page;
                response.data = driverDeails;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching drivers. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
