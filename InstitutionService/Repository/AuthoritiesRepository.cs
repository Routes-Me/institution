using InstitutionService.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstitutionService.Repository
{
    public class AuthoritiesRepository : IAuthoritiesRepository
    {
        private readonly institutionserviceContext _context;
        private readonly AppSettings _appSettings;

        public AuthoritiesRepository(IOptions<AppSettings> appSettings, institutionserviceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }
        public dynamic DeleteAuthorities(string id)
        {
            try
            {
                int authoritiesDecrypted = Obfuscation.Decode(id);
                var authorities = _context.Authorities.Where(x => x.AuthorityId == authoritiesDecrypted).FirstOrDefault();
                if (authorities == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.AuthoritiesNotFound, StatusCodes.Status404NotFound);

                _context.Authorities.Remove(authorities);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.AuthoritiesDelete, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetAuthorities(string authorityId, Pagination pageInfo)
        {
            AuthoritiesGetResponse response = new AuthoritiesGetResponse();
            int totalCount = 0;
            try
            {
                List<AuthoritiesModel> authoritiesModelList = new List<AuthoritiesModel>();
                if (string.IsNullOrEmpty(authorityId))
                {
                    authoritiesModelList = (from authority in _context.Authorities
                                            select new AuthoritiesModel()
                                            {
                                                AuthorityId = Obfuscation.Encode(authority.AuthorityId),
                                                InstitutionId = Obfuscation.Encode(Convert.ToInt32(authority.InstitutionId)),
                                                Pin = authority.Pin
                                            }).AsEnumerable().OrderBy(a => a.AuthorityId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Authorities.ToList().Count();
                }
                else
                {
                    int authorityIdDecrypted = Obfuscation.Decode(authorityId);
                    authoritiesModelList = (from authority in _context.Authorities
                                            where authority.AuthorityId == authorityIdDecrypted
                                            select new AuthoritiesModel()
                                            {
                                                AuthorityId = Obfuscation.Encode(authority.AuthorityId),
                                                InstitutionId = Obfuscation.Encode(Convert.ToInt32(authority.InstitutionId)),
                                                Pin = authority.Pin
                                            }).AsEnumerable().OrderBy(a => a.AuthorityId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Authorities.Where(x => x.AuthorityId == authorityIdDecrypted).ToList().Count();
                }

                if (authoritiesModelList == null || authoritiesModelList.Count == 0)
                    return ReturnResponse.ErrorResponse(CommonMessage.AuthoritiesNotFound, StatusCodes.Status404NotFound);

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.AuthoritiesRetrived;
                response.pagination = page;
                response.data = authoritiesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic GetAuthoritiesByInstitutionId(string id, Pagination pageInfo)
        {
            AuthoritiesGetResponse response = new AuthoritiesGetResponse();
            int totalCount = 0;
            try
            {
                int institutionDecrypted = Obfuscation.Decode(id);
                List<AuthoritiesModel> authoritiesModelList = new List<AuthoritiesModel>();
                authoritiesModelList = (from authority in _context.Authorities
                                        where authority.InstitutionId == institutionDecrypted
                                        select new AuthoritiesModel()
                                        {
                                            AuthorityId = Obfuscation.Encode(authority.AuthorityId),
                                            InstitutionId = Obfuscation.Encode(Convert.ToInt32(authority.InstitutionId)),
                                            Pin = authority.Pin
                                        }).AsEnumerable().OrderBy(a => a.AuthorityId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                totalCount = _context.Authorities.Where(x => x.InstitutionId == institutionDecrypted).ToList().Count();

                var page = new Pagination
                {
                    offset = pageInfo.offset,
                    limit = pageInfo.limit,
                    total = totalCount
                };

                response.status = true;
                response.message = CommonMessage.AuthoritiesRetrived;
                response.pagination = page;
                response.data = authoritiesModelList;
                response.statusCode = StatusCodes.Status200OK;
                return response;
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic InsertAuthorities(AuthoritiesModel model)
        {
            try
            {
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                int institutionIdDecrypted = Obfuscation.Decode(model.InstitutionId);
                var institution = _context.Institutions.Where(x => x.InstitutionId == institutionIdDecrypted).FirstOrDefault();
                if (institution == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.InstitutionNotFound, StatusCodes.Status404NotFound);

                Authorities authority = new Authorities()
                {
                    InstitutionId = institutionIdDecrypted,
                    Pin = model.Pin
                };
                _context.Authorities.Add(authority);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.AuthoritiesInsert, true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }

        public dynamic UpdateAuthorities(AuthoritiesModel model)
        {
            try
            {
                int authoritiesIdIdDecrypted = Obfuscation.Decode(model.AuthorityId);
                int institutionIdDecrypted = Obfuscation.Decode(model.InstitutionId);
                if (model == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.BadRequest, StatusCodes.Status400BadRequest);

                var authorities = _context.Authorities.Where(x => x.AuthorityId == authoritiesIdIdDecrypted).FirstOrDefault();
                if (authorities == null)
                    return ReturnResponse.ErrorResponse(CommonMessage.AuthoritiesNotFound, StatusCodes.Status404NotFound);

                authorities.InstitutionId = institutionIdDecrypted;
                authorities.Pin = model.Pin;
                _context.Authorities.Update(authorities);
                _context.SaveChanges();
                return ReturnResponse.SuccessResponse(CommonMessage.AuthoritiesUpdate, false);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
