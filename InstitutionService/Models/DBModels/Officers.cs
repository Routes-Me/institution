using System;
using System.Collections.Generic;

namespace InstitutionService.Models.DBModels
{
    public partial class Officers
    {
        public Officers()
        {
            Invitations = new HashSet<Invitations>();
        }

        public int OfficerId { get; set; }
        public int? UserId { get; set; }
        public int? InstitutionId { get; set; }

        public virtual Institutions Institution { get; set; }
        public virtual ICollection<Invitations> Invitations { get; set; }
    }
}
