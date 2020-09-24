namespace InstitutionService.Models.ResponseModel
{
    public class InvitationsGetModel
    {
        public string InvitationId { get; set; }
        public string RecipientName { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public byte[] Data { get; set; }
        public string OfficerId { get; set; }
    }
}
