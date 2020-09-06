using Microsoft.AspNetCore.Mvc;

namespace InstitutionService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Institution service started successfully.";
        }
    }
}
