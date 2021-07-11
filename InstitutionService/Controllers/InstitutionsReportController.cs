using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstitutionService.Abstraction;
using InstitutionService.Models;

namespace InstitutionService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
    public class InstitutionsReportController : ControllerBase
    {
        private readonly IInstitutionsReportRepository _institutionReportRepository;

        public InstitutionsReportController(IInstitutionsReportRepository institutionReportRepository)
        {
            _institutionReportRepository = institutionReportRepository;
        }

        [HttpPost]
        [Route("institutions/reports")]
        public IActionResult ReportInstitutions(List<string> institutionIds, [FromQuery] List<string> attr)
        {
            InstitutionGetResponse response = new InstitutionGetResponse();
            try
            {
                response.Data = _institutionReportRepository.ReportInstitutions(institutionIds, attr);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorMessage { Error = ex.Message });
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
