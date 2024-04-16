using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
	// Reporting Structure is a convenience class that handles getting an
	// Employee's data along with the number of reports that they handle
	// In this case, number of reports represents all reports that are either
	// Direct reports, or direct reports of direct reports, but not anyone
	// further down the chain
	public class ReportingStructure
	{
		public String Employee { get; set; }
		public int NumberOfReports { get; set; }
	}
}

