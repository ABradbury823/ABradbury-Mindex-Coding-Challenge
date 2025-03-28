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

                // found an invalid employee id
                if (employee == null) 
                    return null;

                visitedEmployees.Add(employeeId, employee);

                if (employee.DirectReports == null || !employee.DirectReports.Any())
                    continue;
                
                for(int i = 0; i < employee.DirectReports.Count; i++)
                {
                    var dr = employee.DirectReports[i];
                    
                    // account for a looping structure while keeping hierarchy
                    if (visitedEmployees.ContainsKey(dr.EmployeeId))
                    {
                        employee.DirectReports.Remove(dr);
                        employee.DirectReports.Add(new Employee
                        {
                            EmployeeId = dr.EmployeeId,
                            FirstName = dr.FirstName + "(Loop)",
                            LastName = dr.LastName,
                            Position = dr.Position,
                            Department = dr.Department,
                            DirectReports = null
                        });
                        continue;
                    }

                    employeeIds.Push(dr.EmployeeId);
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