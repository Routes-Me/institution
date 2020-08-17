﻿using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Servicesinstitutions
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public int ServiceId { get; set; }

        public virtual Institutions Institution { get; set; }
        public virtual Services Service { get; set; }
    }
}
