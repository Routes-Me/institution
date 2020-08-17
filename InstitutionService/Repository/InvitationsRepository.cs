using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using InstitutionService.Helper;
using Microsoft.Extensions.WebEncoders.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstitutionService.Helper.Abstraction;
using System.Net;

namespace InstitutionService.Repository
{
    public class InvitationsRepository : IInvitationsRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IHelperRepository _helperRepository;
        public InvitationsRepository(institutionserviceContext context, IHelperRepository helperRepository)
        {
            _context = context;
            _helperRepository = helperRepository;
        }

        public InvitationsResponse DeleteInvitation(int officerId, int id)
        {
            InvitationsResponse response = new InvitationsResponse();
            try
            {
                var officer = _context.Officers.Where(x => x.OfficerId == officerId).FirstOrDefault();
                if (officer == null)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                var invitation = _context.Invitations.Where(x => x.InvitationId == id && x.OfficerId == officerId).FirstOrDefault();
                if (invitation == null)
                {
                    response.status = false;
                    response.message = "Invitation not found.";
                    response.responseCode = ResponseCode.NotFound;
                }

                _context.Invitations.Remove(invitation);
                _context.SaveChanges();
                response.status = true;
                response.message = "Invitation deleted successfully.";
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while deleting invitation. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public InvitationsGetResponse GetInvitation(int officerId, int invitationId, PageInfo pageInfo)
        {
            InvitationsGetResponse response = new InvitationsGetResponse();
            InvitationsDetails invitationsDetails = new InvitationsDetails();
            int totalCount = 0;
            try
            {
                var officer = _context.Officers.Where(x => x.OfficerId == officerId).FirstOrDefault();
                if (officer == null)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                List<InvitationsGetModel> objInvitationsModelList = new List<InvitationsGetModel>();
                if (invitationId == 0)
                {
                    objInvitationsModelList = (from invitation in _context.Invitations
                                               where invitation.OfficerId == officerId
                                               select new InvitationsGetModel()
                                               {
                                                   InvitationId = invitation.InvitationId,
                                                   RecipientName = invitation.RecipientName,
                                                   Link = invitation.Link,
                                                   Address = invitation.Address,
                                                   Data = invitation.Data,
                                                   OfficerId = invitation.OfficerId
                                               }).OrderBy(a => a.InvitationId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Invitations.Where(x => x.OfficerId == officerId).ToList().Count();
                }
                else
                {
                    objInvitationsModelList = (from invitation in _context.Invitations
                                               where invitation.OfficerId == officerId && invitation.InvitationId == invitationId
                                               select new InvitationsGetModel()
                                               {
                                                   InvitationId = invitation.InvitationId,
                                                   RecipientName = invitation.RecipientName,
                                                   Link = invitation.Link,
                                                   Address = invitation.Address,
                                                   Data = invitation.Data,
                                                   OfficerId = invitation.OfficerId
                                               }).OrderBy(a => a.InvitationId).Skip((pageInfo.currentPage - 1) * pageInfo.pageSize).Take(pageInfo.pageSize).ToList();

                    totalCount = _context.Invitations.Where(x => x.OfficerId == officerId && x.InvitationId == invitationId).ToList().Count();
                }
                if (objInvitationsModelList == null || objInvitationsModelList.Count == 0)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.data = null;
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }
                invitationsDetails.Invitations = objInvitationsModelList;
                var page = new Pagination
                {
                    offset = pageInfo.currentPage,
                    limit = pageInfo.pageSize,
                    total = totalCount
                };

                response.status = true;
                response.message = "Invitations data retrived successfully.";
                response.pagination = page;
                response.data = invitationsDetails;
                response.responseCode = ResponseCode.Success;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while fetching invitations. Error Message - " + ex.Message;
                response.data = null;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }

        public async Task<InvitationsResponse> InsertInvitation(int OfficerId, InvitationsModel model)
        {
            InvitationsResponse response = new InvitationsResponse();
            try
            {
                byte[] Data;
                string Address = string.Empty, Hash = string.Empty;
                bool IsEmail = false;

                if (OfficerId == 0)
                {
                    response.status = false;
                    response.message = "Pass valid officerid.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                var officer = _context.Officers.Where(x => x.OfficerId == OfficerId).FirstOrDefault();
                if (officer == null)
                {
                    response.status = false;
                    response.message = "Officer not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                if (model == null)
                {
                    response.status = false;
                    response.message = "Pass valid data in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }

                if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
                {
                    response.status = false;
                    response.message = "Pass valid email or phone in model.";
                    response.responseCode = ResponseCode.BadRequest;
                    return response;
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    Address = model.Phone;
                    IsEmail = false;
                }
                else
                {
                    Address = model.Email;
                    IsEmail = true;
                }

                var institution = _context.Institutions.Where(x => x.InstitutionId == model.InstitutionId).FirstOrDefault();
                if (institution == null)
                {
                    response.status = false;
                    response.message = "Institution not found.";
                    response.responseCode = ResponseCode.NotFound;
                    return response;
                }

                Data = Encoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(model));
                Invitations objInvitations = new Invitations()
                {
                    OfficerId = OfficerId,
                    Address = Address,
                    RecipientName = model.RecipientName,
                    Data = Data
                };

                _context.Invitations.Add(objInvitations);
                _context.SaveChanges();

                var encryptedInstitutionId = CryptographyHelper.Encrypt(objInvitations.InvitationId.ToString());

                objInvitations.Link = "routesdashboard.com?invitationid=" + encryptedInstitutionId;
                _context.Invitations.Update(objInvitations);
                _context.SaveChanges();

                if (IsEmail)
                {
                    var res = await _helperRepository.SendEmail(objInvitations.Link, model.Email);
                    if (res.StatusCode != HttpStatusCode.Accepted)
                    {
                        response.status = false;
                        response.message = "Something went wrong while sending invitation email. Error Message -" + res.Body.ReadAsStringAsync();
                        response.responseCode = ResponseCode.InternalServerError;
                        return response;
                    }
                }
                else
                {
                    //do something here to send SMS
                }
                response.status = true;
                response.message = "Invitation sent successfully.";
                response.responseCode = ResponseCode.Created;
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "Something went wrong while sending invitation. Error Message - " + ex.Message;
                response.responseCode = ResponseCode.InternalServerError;
                return response;
            }
        }
    }
}
