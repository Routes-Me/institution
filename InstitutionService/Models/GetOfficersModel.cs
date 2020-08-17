using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models
{
    public class GetOfficersModel
    {
        public int OfficerId { get; set; }
        public int? UserId { get; set; }
        public int? InstitutionId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
}
