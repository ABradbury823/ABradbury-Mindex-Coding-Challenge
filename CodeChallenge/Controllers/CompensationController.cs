using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee/{employeeId}/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger<CompensationController> _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _compensationService = compensationService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '" +
                $"{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationByEmployeeId",
                new { employeeId = compensation.Employee.EmployeeId },
                compensation
                );
        }

        [HttpGet(Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if(compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}