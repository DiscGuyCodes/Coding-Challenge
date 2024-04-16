using CodeChallenge.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureServiceTests
    {
        private static Mock<IEmployeeRepository> _mockEmployeeRepository;
        private static Mock<ILogger<ReportingStructureService>> _mockLogger;
        private static IReportingStructureService _reportingStructureService;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockLogger = new Mock<ILogger<ReportingStructureService>>();
            _reportingStructureService = new ReportingStructureService(_mockLogger.Object, _mockEmployeeRepository.Object);
        }

        // This test verify's that ReportingStructureRepository correctly calls
        // the GetById method from EmployeeRepository and calculates
        // ReportingStructure from it
        [TestMethod]
        public async Task GetReportingStructureByEmployeeId_Returns_CorrectData_For_ValidEmployee()
        {
            // Arrange
            // The employee below is John Lennon from the Seed data, with an additional third-ary report Caleb Kellicutt
            Employee employee = new Employee
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                FirstName = "John",
                LastName = "Lennon",
                Position = "Development Manager",
                Department = "Engineering",
                DirectReports = new List<Employee>
                {
                    new Employee
                    {
                        EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                        FirstName = "Paul",
                        LastName = "McCartney",
                        Position = "Developer I",
                        Department = "Engineering",
                        DirectReports = null
                    },
                    new Employee
                    {
                        EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                        FirstName = "Ringo",
                        LastName = "Starr",
                        Position = "Developer V",
                        Department = "Engineering",
                        DirectReports = new List<Employee>{
                            new Employee{
                                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                                FirstName = "Pete",
                                LastName = "Best",
                                Position = "Developer II",
                                Department = "Engineering",
                                DirectReports = null
                            },
                            new Employee{
                                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                                FirstName = "George",
                                LastName = "Harrison",
                                Position = "Developer III",
                                Department = "Engineering",
                                DirectReports = new List<Employee> {
                                    new Employee{
                                        EmployeeId = "new_id_string_01",
                                        FirstName = "Caleb",
                                        LastName = "Kellicutt",
                                        Position = "Relative",
                                        Department = "Engineering",
                                        DirectReports = null
                                    }
                                }
                            }
                        }
                    }
                }
            };
            // Tell the mock Employee Repository what to return to when GetById is requested for John Lennon
            _mockEmployeeRepository.Setup(r => r.GetById("16a596ae-edd3-4847-99fe-c4518e82c86f")).ReturnsAsync(employee);

            //Act
            ReportingStructure reportingStructure = await _reportingStructureService.GetReportingStructureByEmployeeId("16a596ae-edd3-4847-99fe-c4518e82c86f");

            // Assert
            Assert.IsNotNull(reportingStructure);
            Assert.AreEqual(employee.EmployeeId, reportingStructure.Employee);
            Assert.AreEqual(4, reportingStructure.NumberOfReports);

        }


        // Verifies that ReportingStructureService's GetReportingStructureByEmployeeId correctly
        // Passes Null back when attempting Get ReportingStructure for an Invalid Employee ID
        [TestMethod]
        public async Task GetReportingStructureByEmployeeId_Returns_Null_For_InvalidEmployee()
        {
            // Arrange
            // Tell the mock Employee Repository what to return to when GetById is requested for invalid-employee-id
            string invalidEmployeeId = "invalid-employee-id";
            _mockEmployeeRepository.Setup(r => r.GetById(invalidEmployeeId)).ReturnsAsync((Employee)null);

            // Act
            ReportingStructure reportingStructure = await _reportingStructureService.GetReportingStructureByEmployeeId(invalidEmployeeId);

            // Assert
            Assert.IsNull(reportingStructure);
        }
    }
}

