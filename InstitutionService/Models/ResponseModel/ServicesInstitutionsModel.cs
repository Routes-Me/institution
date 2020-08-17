﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class ServicesInstitutionsModel
    {
        public int InstitutionId { get; set; }
        public int ServiceId { get; set; }
    }

    public class ServicesInstitutionsPostModel
    {
        public int id { get; set; }
        public int ServiceId { get; set; }
    }
}
