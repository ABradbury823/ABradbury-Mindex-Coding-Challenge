using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers 
{
    [ApiController]
    [Route("api/reporting-structure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService rsService)
        {
            _logger = logger;
            _reportingStructureService = rsService;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetReportingStructureByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received reporting structure get request for '{employeeId}'");

            var reportingStructure = _reportingStructureService.GetByEmployeeId(employeeId);

            if (reportingStructure == null) 
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}