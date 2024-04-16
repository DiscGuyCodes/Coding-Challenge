using System;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
	public class ReportingStructureService : IReportingStructureService
	{
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
		{
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        // Retrieve the employee using the _employeeRepository and use this to create
        // and return ReportingStructure
        public async Task<ReportingStructure> GetReportingStructureByEmployeeId(string employeeId)
        {
            // Grab the employee and if it is null, then it has not been found
            Employee employee = await _employeeRepository.GetById(employeeId);
            if(employee != null)
            {
                return new ReportingStructure
                {
                    Employee = employee.EmployeeId,
                    NumberOfReports = GetDirectReportCountByEmployee(employee)
                };
            }
            return null;
        }

        // the Method GetDirectReportCountByEmployee counts the number of employees
        // that are direct reports and secondary reports of the given employee
        // a secondary report is a direct report of a direct report of the employee
        private int GetDirectReportCountByEmployee(Employee employee)
        {
            int count = 0;
            // ensure that direct reports is not empty before looping through
            if (employee.DirectReports != null)
            {
                foreach (var directReport in employee.DirectReports)
                {
                    // This is a direct report, so add 1 to the count
                    count += 1;
                    // ensure that direct reports is not empty before looping through
                    if (directReport.DirectReports != null)
                    {
                        foreach (var secondaryReport in directReport.DirectReports)
                        {
                            // This is a secondary report, so add 1 to the count
                            count += 1;
                        }
                    }
                }
            }
            return count;
        }
    }
}

