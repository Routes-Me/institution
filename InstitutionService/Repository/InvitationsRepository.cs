using InstitutionService.Abstraction;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using InstitutionService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstitutionService.Helper.Abstraction;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InstitutionService.Repository
{
    public class InvitationsRepository : IInvitationsRepository
    {
        private readonly institutionserviceContext _context;
        private readonly IHelperRepository _helperRepository;
        private readonly IMessageSender _messageSender;
        public InvitationsRepository(institutionserviceContext context, IHelperRepository helperRepository, IMessageSender messageSender)
        {
            _context = context;
            _helperRepository = helperRepository;
            _messageSender = messageSender;
        }

        public dynamic DeleteInvitation(string officerId, string id)
        {
            try
            {
                var officer = _context.Officers.Include(x => x.Invitations).Where(x => x.OfficerId == Convert.ToInt32(officerId)).FirstOrDefault();
                if (officer == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                var invitation = officer.Invitations.Where(x => x.InvitationId == Convert.ToInt32(id) && x.OfficerId == Convert.ToInt32(officerId)).FirstOrDefault();
                if (invitation == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InvitationNotFound, StatusCodes.Status404NotFound);

                _context.Invitations.Remove(invitation);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.InvitationDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetInvitation(string invitationId, Pagination pageInfo)
        {
            InvitationsGetResponse response = new InvitationsGetResponse();
            int totalCount = 0;
            try
            {
                List<InvitationsGetModel> objInvitationsModelList = new List<InvitationsGetModel>();
                if (invitationId == "0")
                {
                    objInvitationsModelList = (from invitation in _context.Invitations

                                               select new InvitationsGetModel()
                                               {
                                                   InvitationId = invitation.InvitationId.ToString(),
                                                   RecipientName = invitation.RecipientName,
                                                   Link = invitation.Link,
                                                   Address = invitation.Address,
                                                   Data = invitation.Data,
                                                   OfficerId = invitation.OfficerId.ToString()
                                               }).OrderBy(a => a.InvitationId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Invitations.ToList().Count();
                }
                else
                {
                    objInvitationsModelList = (from invitation in _context.Invitations
                                               where invitation.InvitationId == Convert.ToInt32(invitationId)
                                               select new InvitationsGetModel()
                                               {
                                                   InvitationId = invitation.InvitationId.ToString(),
                                                   RecipientName = invitation.RecipientName,
                                                   Link = invitation.Link,
                                                   Address = invitation.Address,
                                                   Data = invitation.Data,
                                                   OfficerId = invitation.OfficerId.ToString()
                                               }).OrderBy(a => a.InvitationId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Invitations.Where(x => x.InvitationId == Convert.ToInt32(invitationId)).ToList().Count();
                }

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.InvitationRetrived;
                response.pagination = page;
                response.data = objInvitationsModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public async Task<dynamic> InsertInvitation(string OfficerId, InvitationsModel model)
        {
            try
            {
                byte[] Data;
                string Address = string.Empty, Hash = string.Empty;
                bool IsEmail = false;

                var officer = _context.Officers.Where(x => x.OfficerId == Convert.ToInt32(OfficerId)).FirstOrDefault();
                if (officer == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.OfficerNotFound, StatusCodes.Status404NotFound);

                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);
                
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

                var institution = _context.Institutions.Where(x => x.InstitutionId == Convert.ToInt32(model.InstitutionId)).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InvitationNotFound, StatusCodes.Status404NotFound);

                Data = Encoding.ASCII.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(model));
                Invitations invitation = new Invitations()
                {
                    OfficerId = Convert.ToInt32(OfficerId),
                    Address = Address,
                    RecipientName = model.Name,
                    Data = Data
                };

                _context.Invitations.Add(invitation);
                _context.SaveChanges();

                var encryptedInstitutionId = CryptographyHelper.Encrypt(invitation.InvitationId.ToString());

                invitation.Link = "routesdashboard.com?invitationid=" + encryptedInstitutionId;
                _context.Invitations.Update(invitation);
                _context.SaveChanges();

                if (IsEmail)
                {
                    var res = await _helperRepository.SendEmail(invitation.Link, model.Email);
                    if (res.StatusCode != HttpStatusCode.Accepted)
                        throw new Exception(res.Body.ReadAsStringAsync().ToString());
                }
                else
                {
                    var res = await _messageSender.SendMessage(model.Phone, "Invitation Link: " + invitation.Link);
                }

                return ReturnResponse.SuccessResponse(CommonMessage.InvitationInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
