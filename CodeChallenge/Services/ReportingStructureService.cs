using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return ConstructReportingStructure(employeeId);
            }
            return null;
        }

        // Implemented using depth-first traversal, in case the reporting hierarchy is circular
        private ReportingStructure ConstructReportingStructure(String rootEmployeeId)
        {
            Stack<String> employeeIds = new Stack<String>();
            Dictionary<String, Employee> visitedEmployees = new Dictionary<String, Employee>();

            employeeIds.Push(rootEmployeeId);

            while(employeeIds.Any())
            {
                var employeeId = employeeIds.Pop();

                var employee = _employeeRespository.GetById(employeeId, true);
                visitedEmployees.Add(employeeId, employee);

                if (employee.DirectReports != null && employee.DirectReports.Any())
                {
                    foreach(var dr in employee.DirectReports)
                    {
                        // account for a looping structure
                        if (visitedEmployees.ContainsKey(dr.EmployeeId)) continue;

                        employeeIds.Push(dr.EmployeeId);
                    }
                }
            }

            var reportingStructure = new ReportingStructure
            {
                Employee = visitedEmployees[rootEmployeeId],
                NumberOfReports = visitedEmployees.Count - 1 // don't count yourself
            };

            return reportingStructure;
        }
    }
}