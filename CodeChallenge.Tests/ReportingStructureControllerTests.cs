using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using CodeChallenge.Models;
using CodeChallenge.Tests.Integration.Extensions;
using CodeChallenge.Tests.Integration.Helpers;
using System.Collections.Generic;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private HttpClient _httpClient;
        private TestServer _testServer;

        [TestInitialize]
        public void InitializeTest()
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }


        // This test directly adds an Employee to the in memory DB
        // and verifies that the ReportingStructure returned from a GET call
        // is correct for the given employee
        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f-manual";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedDirectReportCount = 4;

            // The employee below is John Lennon from the Seed data, with an additional third-ary report Caleb Kellicutt
            Employee employee = new Employee
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f-manual",
                FirstName = "John",
                LastName = "Lennon",
                Position = "Development Manager",
                Department = "Engineering",
                DirectReports = new List<Employee>
                {
                    new Employee
                    {
                        EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3-manual",
                        FirstName = "Paul",
                        LastName = "McCartney",
                        Position = "Developer I",
                        Department = "Engineering",
                        DirectReports = null
                    },
                    new Employee
                    {
                        EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f-manual",
                        FirstName = "Ringo",
                        LastName = "Starr",
                        Position = "Developer V",
                        Department = "Engineering",
                        DirectReports = new List<Employee>{
                            new Employee{
                                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309-manual",
                                FirstName = "Pete",
                                LastName = "Best",
                                Position = "Developer II",
                                Department = "Engineering",
                                DirectReports = null
                            },
                            new Employee{
                                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c-manual",
                                FirstName = "George",
                                LastName = "Harrison",
                                Position = "Developer III",
                                Department = "Engineering",
                                DirectReports = new List<Employee> {
                                    new Employee{
                                        EmployeeId = "new_id_string_01-manual",
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

            // Add the employee to the database directly
            _testServer.AddEmployee(employee);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/structure/{employeeId}");
            var getResponse = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            var reportStructure = getResponse.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(employeeId, reportStructure.Employee);
            Assert.AreEqual(expectedDirectReportCount, reportStructure.NumberOfReports);
        }

        // This method verifys that on an Invalid EmployeeID
        // GetReportingStructureByEmployeeId Returns NotFound
        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_NotFound()
        {
            // Arrange
            string invalidEmployeeId = "invalid";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/structure/{invalidEmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

}