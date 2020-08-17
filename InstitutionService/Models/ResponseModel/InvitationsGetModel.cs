using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionService.Models.ResponseModel
{
    public class InvitationsGetModel
    {
        public int InvitationId { get; set; }
        public string RecipientName { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public byte[] Data { get; set; }
        public int? OfficerId { get; set; }
    }
}
