﻿using InstitutionService.Models;
using InstitutionService.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Abstraction
{
    public interface IInstitutionRepository
    {
        InstitutionResponse InsertInstitutions(InstitutionsModel institutionModel);
        InstitutionResponse UpdateInstitution(InstitutionsModel institutionModel);
        InstitutionResponse DeleteInstitution(int id);
        InstitutionGetResponse GetInstitutions(int institutionId, PageInfo pageInfo);
        InstitutionVehicleResponse GetVehicles(int institutionId, int vehicleId, PageInfo pageInfo);
        InstitutionDriverResponse GetDrivers(int institutionId, int vehicleId, int driverId, PageInfo pageInfo);
    }
}