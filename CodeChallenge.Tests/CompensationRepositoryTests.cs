using System;
using CodeChallenge.Models;
using CodeChallenge.Data;
using CodeChallenge.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationRepositoryTests
	{
        private CompensationContext _compensationContext;
        private CompensationRepository _compensationRepository;
        private static Mock<ILogger<ICompensationRepository>> _mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CompensationContext>()
                .UseInMemoryDatabase("CompensationDB")
                .Options;

            _mockLogger = new Mock<ILogger<ICompensationRepository>>();
            _compensationContext = new CompensationContext(options);
            _compensationRepository = new CompensationRepository(_mockLogger.Object, _compensationContext);
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            _compensationContext.Dispose();
        }

        // This use the compensation repository to create a compensation and then
        // attempts to find that compensation from the database directly
        [TestMethod]
        public void CreateCompensation_And_Verify()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation
            {
                EmployeeId = employeeId,
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };

            // Act
            _compensationRepository.Add(compensation);
            _compensationContext.SaveChangesAsync();

            // Assert
            var createdCompensation = _compensationContext.Compensations.FirstOrDefault(c => c.EmployeeId == employeeId);
            Assert.IsNotNull(createdCompensation);
            Assert.AreEqual(compensation.Salary, createdCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, createdCompensation.EffectiveDate);
        }


        // This test adds a compensation directly to the DB and then
        // calls GetCompensationByEmployeeId and verify's the values
        // are the same for the created and retrieved
        [TestMethod]
        public void GetCompensationByEmployeeID_On_Valid_EmployeeId()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f-valid";
            var compensation = new Compensation
            {
                EmployeeId = employeeId,
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };

            // Act
            _compensationContext.Add(compensation);
            _compensationContext.SaveChangesAsync();

            // Assert
            var createdCompensation = _compensationRepository.GetCompensationByEmployeeId(employeeId).Result;
            Assert.IsNotNull(createdCompensation);
            Assert.AreEqual(compensation.Salary, createdCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, createdCompensation.EffectiveDate);
        }


        // This Test Method ensures that a GetCompensationByEmployeeID
        // for an invalid EmployeeID returns null
        [TestMethod]
        public void GetCompensationByEmployeeID_On_Invalid_EmployeeId()
        {
            // Arrange
            var employeeId = "invalid";

            // Act

            // Assert
            var createdCompensation = _compensationRepository.GetCompensationByEmployeeId(employeeId).Result;
            Assert.IsNull(createdCompensation);
        }

        // This creates a compensation the same way as in teh above test,
        // but now uses the compensation repository's GetCompensationByEmployeeId
        // to read the compensation and compare it to the created compensation
        // Acts as an 'end to end' test here
        [TestMethod]
        public void CreateCompensation_Then_ReadCompensation()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation
            {
                EmployeeId = employeeId,
                Salary = 100000,
                EffectiveDate = DateTime.Now
            };

            // Act
            _compensationRepository.Add(compensation);
            _compensationContext.SaveChangesAsync();

            // Assert
            var createdCompensation = _compensationRepository.GetCompensationByEmployeeId(employeeId).Result;
            Assert.IsNotNull(createdCompensation);
            Assert.AreEqual(compensation.Salary, createdCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, createdCompensation.EffectiveDate);
        }
    }
}