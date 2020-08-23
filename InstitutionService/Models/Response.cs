using InstitutionService.Models.ResponseModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public InstitutionDetails data { get; set; }
    }

    public class InstitutionDetails
    {
        public List<GetInstitutionsModel> institutions { get; set; }
    }

    public class InstitutionVehicleResponse : Response
    {
        public Pagination pagination { get; set; }
        public InstitutionVehicleDetails data { get; set; }
    }

    public class InstitutionVehicleDetails
    {
        public List<VehiclesModel> vehicles { get; set; }
    }

    public class InstitutionDriverResponse : Response
    {
        public Pagination pagination { get; set; }
        public InstitutionDriverDetails data { get; set; }
    }

    public class InstitutionDriverDetails
    {
        public List<DriversModel> drivers { get; set; }
    }

    #endregion

    #region Services Response

    public class ServicesResponse : Response { }

    public class ServicesGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public ServicesDetails data { get; set; }
    }

    public class ServicesDetails
    {
        public List<ServicesModel> services { get; set; }
    }
    #endregion

    #region Officers Response

    public class OfficersResponse : Response { }
    public class OfficersGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public OfficersDetails data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }

    public class OfficersDetails
    {
        public List<OfficersModel> officers { get; set; }
    }

    public class UserData
    {
        public Pagination pagination { get; set; }
        public UserDetails data { get; set; }
    }

    public class UserDetails
    {
        public List<UserModel> users { get; set; }
    }

    #endregion

    #region ServicesInstitutions Response
    public class ServicesInstitutionsResponse : Response { }

    public class ServicesInstitutionsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public ServicesInstitutionsDetails data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject included { get; set; }
    }

    public class ServicesInstitutionsDetails
    {
        public List<ServicesInstitutionsModel> servicesInstitutions { get; set; }
    }
    #endregion

    #region Invitations Response
    public class InvitationsResponse : Response { }

    public class InvitationsGetResponse : Response
    {
        public Pagination pagination { get; set; }
        public InvitationsDetails data { get; set; }
    }

    public class InvitationsDetails
    {
        public List<InvitationsGetModel> Invitations { get; set; }
    }
    #endregion
}