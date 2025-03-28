using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRespository;
        private readonly ILogger _logger;

        public ReportingStructureService(IEmployeeRepository employeeRespository, ILogger logger)
        {
            _employeeRespository = employeeRespository;
            _logger = logger;
        }

        public ReportingStructure GetByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                ReportingStructure reportingStructure = new ReportingStructure();
                reportingStructure.Employee = _employeeRespository.GetById(employeeId);
                if (reportingStructure.Employee != null)
                {
                    reportingStructure.NumberOfReports = NumberOfReports(reportingStructure.Employee);
                }
                return reportingStructure;
            }
            return null;
        }

        // Helper function for calculating number of reports
        // We'll do it recursively first, which makes the assumption that direct reporting cannot be circular.
        // We can then implement breadth-first traversal to account for circular reporting.
        private int NumberOfReports(Employee employee) 
        {
            int num = 0;

            if (employee.DirectReports != null)
            {
                num += employee.DirectReports.Count;
                num += NumberOfReports(employee);
            }
            return num;
        }
    }
}