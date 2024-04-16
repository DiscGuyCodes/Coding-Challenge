using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reporting/structure")] // separating reporting and structure here in anticipation of potential future augmentations
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
		{
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        // Reporting Structure Get with EmployeeId passed in
        [HttpGet("{employeeId}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetReportingStructureByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received reporting structure get request for employee '{employeeId}'");

            var reportingStructure = _reportingStructureService.GetReportingStructureByEmployeeId(employeeId).Result;

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}

