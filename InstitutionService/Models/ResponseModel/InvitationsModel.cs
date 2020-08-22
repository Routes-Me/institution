using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class InvitationsModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<int> Roles { get; set; }
        public int? InstitutionId { get; set; }
    }
}
