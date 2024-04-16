using CodeChallenge.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Tests.Integration
{
	[TestClass]
	public class CompensationServiceTests
	{
        private static Mock<ICompensationRepository> _mockCompensationRepository;
        private static Mock<ILogger<CompensationService>> _mockLogger;
        private static ICompensationService _compensationService;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            _mockCompensationRepository = new Mock<ICompensationRepository>();
            _mockLogger = new Mock<ILogger<CompensationService>>();
            _compensationService = new CompensationService(_mockLogger.Object, _mockCompensationRepository.Object);
        }


        // This test verify's that CompensationRepository correctly calls the
        // GetCompensationByEmployeeId method from CompensationRepository
        // and returns it by comparing the result Compensation to the Mock
        // CompensationRepositories result Compensation
        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_CorrectData_For_ValidEmployee()
        {
            // Arrange
            Compensation compensation = new Compensation
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };
            // Tell the mock Employee Repository what to return to when GetById is requested for John Lennon
            _mockCompensationRepository.Setup(r => r.GetCompensationByEmployeeId("16a596ae-edd3-4847-99fe-c4518e82c86f")).ReturnsAsync(compensation);

            //Act
            Compensation compensationResult = _compensationService.GetCompensationByEmployeeId("16a596ae-edd3-4847-99fe-c4518e82c86f");

            // Assert
            Assert.IsNotNull(compensationResult);
            Assert.AreEqual(compensation.EmployeeId, compensationResult.EmployeeId);
            Assert.AreEqual(compensation.Salary, compensationResult.Salary);
            Assert.AreEqual(compensation.EffectiveDate, compensationResult.EffectiveDate);
        }


        // Verifies that CompensationService's GetCompensationByEmployeeId correctly
        // Passes Null back when attempting Get Compensation by an Invalid Employee ID
        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_Null_For_InvalidEmployeeId()
        {
            // Arrange
            // Tell the mock Employee Repository what to return to when GetById is requested for invalid-employee-id
            string invalidEmployeeId = "invalid-employee-id";
            _mockCompensationRepository.Setup(r => r.GetCompensationByEmployeeId(invalidEmployeeId)).ReturnsAsync((Compensation)null);

            // Act
            Compensation compensationResult = _compensationService.GetCompensationByEmployeeId(invalidEmployeeId);

            // Assert
            Assert.IsNull(compensationResult);
        }


        // Verifies that return value when creating a Compensation is the same
        // as the compensation that was originally passed in
        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };
            

            //Act
            Compensation compensationResult = _compensationService.CreateCompensation(compensation);

            // Assert
            Assert.IsNotNull(compensationResult);
            Assert.AreEqual(compensation.EmployeeId, compensationResult.EmployeeId);
            Assert.AreEqual(compensation.Salary, compensationResult.Salary);
            Assert.AreEqual(compensation.EffectiveDate, compensationResult.EffectiveDate);
        }

        // Verifies that the CreateCompensation method calls CompensationRepositories
        // Add method one time
        [TestMethod]
        public void CreateCompensation_Verify_Calls_CompensationRepositoryAdd()
        {
            // Arrange
            Compensation compensation = new Compensation
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };


            //Act
            Compensation compensationResult = _compensationService.CreateCompensation(compensation);

            // Assert
            _mockCompensationRepository.Verify(r => r.Add(compensation), Times.Once);
        }
    }
}

