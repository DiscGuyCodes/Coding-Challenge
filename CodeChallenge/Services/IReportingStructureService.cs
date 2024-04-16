using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
	public interface IReportingStructureService
	{
		// The only current method for Reporting Structure Service
		// This will get the ReportingStructure for the Employee given (by their Id)
		Task<ReportingStructure> GetReportingStructureByEmployeeId(String employeeId);
	}
}

