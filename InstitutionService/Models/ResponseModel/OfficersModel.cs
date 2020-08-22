using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class OfficersModel
    {
        public int OfficerId { get; set; }
        public int? UserId { get; set; }
        public int? InstitutionId { get; set; }
    }
}
