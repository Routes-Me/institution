using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Invitations
    {
        public int InvitationId { get; set; }
        public string RecipientName { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public byte[] Data { get; set; }
        public int? OfficerId { get; set; }

        public virtual Officers Officer { get; set; }
    }
}
