using InstitutionService.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Models;
using InstitutionService.Models.DBModels;
using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Obfuscation;
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
                int authoritiesDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(id), _appSettings.PrimeInverse);
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

        public dynamic GetAuthorities(string id, Pagination pageInfo)
        {
            int authoritiesDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(id), _appSettings.PrimeInverse);
            AuthoritiesGetResponse response = new AuthoritiesGetResponse();
            int totalCount = 0;
            try
            {

                List<AuthoritiesModel> authoritiesModelList = new List<AuthoritiesModel>();
                if (authoritiesDecrypted == 0)
                {
                    authoritiesModelList = (from authority in _context.Authorities
                                            select new AuthoritiesModel()
                                            {
                                                AuthorityId = ObfuscationClass.EncodeId(authority.AuthorityId, _appSettings.Prime).ToString(),
                                                InstitutionId = ObfuscationClass.EncodeId(Convert.ToInt32(authority.InstitutionId), _appSettings.Prime).ToString(),
                                                Pin = authority.Pin
                                            }).AsEnumerable().OrderBy(a => a.AuthorityId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Authorities.ToList().Count();
                }
                else
                {
                    authoritiesModelList = (from authority in _context.Authorities
                                            where authority.AuthorityId == authoritiesDecrypted
                                            select new AuthoritiesModel()
                                            {
                                                AuthorityId = ObfuscationClass.EncodeId(authority.AuthorityId, _appSettings.Prime).ToString(),
                                                InstitutionId = ObfuscationClass.EncodeId(Convert.ToInt32(authority.InstitutionId), _appSettings.Prime).ToString(),
                                                Pin = authority.Pin
                                            }).AsEnumerable().OrderBy(a => a.AuthorityId).Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();

                    totalCount = _context.Authorities.Where(x => x.AuthorityId == authoritiesDecrypted).ToList().Count();
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
                int institutionDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(id), _appSettings.PrimeInverse);
                List<AuthoritiesModel> authoritiesModelList = new List<AuthoritiesModel>();
                authoritiesModelList = (from authority in _context.Authorities
                                        where authority.InstitutionId == institutionDecrypted
                                        select new AuthoritiesModel()
                                        {
                                            AuthorityId = ObfuscationClass.EncodeId(authority.AuthorityId, _appSettings.Prime).ToString(),
                                            InstitutionId = ObfuscationClass.EncodeId(Convert.ToInt32(authority.InstitutionId), _appSettings.Prime).ToString(),
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

                int institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(model.InstitutionId), _appSettings.PrimeInverse);
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
                int authoritiesIdIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(model.AuthorityId), _appSettings.PrimeInverse);
                int institutionIdDecrypted = ObfuscationClass.DecodeId(Convert.ToInt32(model.InstitutionId), _appSettings.PrimeInverse);
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
