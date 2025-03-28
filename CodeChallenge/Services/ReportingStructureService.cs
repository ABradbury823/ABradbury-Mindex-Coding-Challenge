using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRespository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRespository)
        {
            _employeeRespository = employeeRespository;
            _logger = logger;
        }

        public ReportingStructure GetByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                var employee = _employeeRespository.GetById(employeeId);
                if (employee != null)
                {
                    ReportingStructure reportingStructure = new ReportingStructure();
                    reportingStructure.Employee = employee;
                    reportingStructure.NumberOfReports = NumberOfReports(reportingStructure.Employee);
                    return reportingStructure;
                }
            }
            return null;
        }

        // Implemented recursively under the assumption that the reporting structure is non-circular
        // We can then implement depth-first traversal to account for circular reporting.
        private int NumberOfReports(Employee employee) 
        {
            int num = 0;

            if (employee.DirectReports != null)
            {
                num += employee.DirectReports.Count;
                foreach (var e in employee.DirectReports)
                {
                    num += NumberOfReports(e);
                }
            }
            return num;
        }
    }
}