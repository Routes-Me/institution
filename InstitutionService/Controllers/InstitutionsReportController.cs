using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstitutionService.Abstraction;
using InstitutionService.Models;

namespace InstitutionService.Internal.Controllers
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
        public IActionResult ReportInstitutions(List<int> institutionIds, [FromQuery] List<string> attr)
        {
            InstitutionsGetReportDto institutionsGetReportDto = new InstitutionsGetReportDto();
            try
            {
                institutionsGetReportDto.Data = _institutionReportRepository.ReportInstitutions(institutionIds, attr);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorMessage { Error = ex.Message });
            }
            return StatusCode(StatusCodes.Status200OK, institutionsGetReportDto);
        }
    }
}
