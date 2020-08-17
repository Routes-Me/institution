using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class InvitationsModel
    {
        public int InvitationId { get; set; }
        public string RecipientName { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public byte[] Data { get; set; }
        public int? OfficerId { get; set; }
        public int? RoleId { get; set; }
        public int? InstitutionId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Applications { get; set; }
    }
}
