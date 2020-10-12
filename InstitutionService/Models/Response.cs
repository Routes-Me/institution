using InstitutionService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace InstitutionService.Models
{

    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
    }

    public class ReturnResponse
    {
        public static dynamic ExceptionResponse(Exception ex)
        {
            Response response = new Response();
            response.status = false;
            response.message = CommonMessage.GenericException;
            response.statusCode = StatusCodes.Status401Unauthorized;
            return response;
        }

        public static dynamic SuccessResponse(string message, bool isCreated)
        {
            Response response = new Response();
            response.status = true;
            response.message = message;
            if (isCreated)
                response.statusCode = StatusCodes.Status201Created;
            else
                response.statusCode = StatusCodes.Status200OK;
            return response;
        }

        public static dynamic ErrorResponse(string message, int statusCode)
        {
            Response response = new Response();
            response.status = false;
            response.message = message;
            response.statusCode = statusCode;
            return response;
        }
    }

    #region Institution Response

    public class InstitutionGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<InstitutionsModel> data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }
    #endregion

    #region Services Response
    public class ServicesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<ServicesModel> data { get; set; }
    }
    #endregion

    #region Officers Response
    public class OfficersGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<OfficersModel> data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }
    public class UserData
    {
        public Pagination pagination { get; set; }
        public List<UserModel> data { get; set; }
    }
    #endregion

    #region ServicesInstitutions Response

    public class ServicesInstitutionsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<ServicesInstitutionsModel> data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }
    #endregion

    #region Invitations Response
    public class InvitationsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<InvitationsGetModel> data { get; set; }
    }
    #endregion

    #region Authorities Reponse
    public class AuthoritiesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<AuthoritiesModel> data { get; set; }
    }
    #endregion
}