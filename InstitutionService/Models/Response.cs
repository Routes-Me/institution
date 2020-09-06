using InstitutionService.Models.ResponseModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace InstitutionService.Models
{

    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
        public ResponseCode responseCode { get; set; }
    }
    public enum ResponseCode
    {
        Success = 200,
        Error = 2,
        InternalServerError = 500,
        MovedPermanently = 301,
        NotFound = 404,
        BadRequest = 400,
        Conflict = 409,
        Created = 201,
        NotAcceptable = 406,
        Unauthorized = 401,
        RequestTimeout = 408,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        Permissionserror = 403,
        Forbidden = 403,
        TokenRequired = 499,
        InvalidToken = 498
    }

    #region Institution Response

    public class InstitutionResponse : Response
    {

    }

    public class InstitutionGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<GetInstitutionsModel> data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }
    public class InstitutionVehicleResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<VehiclesModel> data { get; set; }
    }

    public class InstitutionDriverResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<DriversModel> data { get; set; }
    }
    #endregion

    #region Services Response

    public class ServicesResponse : Response { }

    public class ServicesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<ServicesModel> data { get; set; }
    }
    #endregion

    #region Officers Response

    public class OfficersResponse : Response { }
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
    public class ServicesInstitutionsResponse : Response { }

    public class ServicesInstitutionsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<ServicesInstitutionsModel> data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }

    #endregion

    #region Invitations Response
    public class InvitationsResponse : Response { }

    public class InvitationsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public List<InvitationsGetModel> data { get; set; }
    }

 
    #endregion
}