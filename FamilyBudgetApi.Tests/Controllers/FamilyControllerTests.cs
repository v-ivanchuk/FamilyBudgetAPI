using FamilyBudgetAPI;
using FamilyBudgetAPI.Controllers;
using FamilyBudgetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyBudgetApi.Tests.Controllers
{
    [TestFixture]
    public class FamilyControllerTests
    {
        private FamilyController familyController;
        private BudgetContext budgetContext;

        [SetUp]
        public void Setup()
        {
            var budgetOptions = new DbContextOptionsBuilder<BudgetContext>()
                .UseInMemoryDatabase(databaseName: "BudgetTest")
                .Options;

            budgetContext = new BudgetContext(budgetOptions);

            budgetContext.Families.AddRange(Families());
            budgetContext.SaveChanges();

            familyController = new FamilyController(budgetContext);
        }

        [TearDown]
        public void Cleanup()
        {
            budgetContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task GetFamilies_CallGetMethod_ReceiveListOfFamilies()
        {
            // Arrange
            var familiesCount = Families().Count;

            // Act
            var familyResponse = await familyController.Get();

            // Assert
            Assert.AreEqual(((List<Family>)familyResponse).Count, familiesCount);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        public async Task GetFamilyById_CallCorrectId_ReceiveFamily(int id)
        {
            // Arrange
            var family = budgetContext.Families.FirstOrDefault(f => f.Id == id);

            // Act
            var familyResponse = await familyController.Get(id);

            // Assert
            Assert.AreEqual(familyResponse.Value, family);
        }

        [Test]
        [TestCase(null)]
        public async Task GetFamilyById_IdAsNull_ReceiveNotFound(int id)
        {
            // Act
            var familyResponse = await familyController.Get(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(familyResponse.Result);
        }

        [Test]
        public async Task PostFamily_CallPostMethodWithValidData_ReceiveOkResult()
        {
            // Arrange
            var family = new Family { Name = "Tkach", DateCreated = DateTime.Now, DateUpdated = DateTime.Now };

            // Act
            var familyResponse = await familyController.Post(family);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(familyResponse.Result);
        }

        [Test]
        public async Task PostFamily_CallPostMethodWithInvalidData_ReceiveBadRequestResult()
        {
            // Arrange
            var existingFamily = budgetContext.Families.FirstOrDefault(f => f.Id == 1);

            // Act
            var familyWithNull = await familyController.Post(null);
            var existingFamilyResponse = await familyController.Post(existingFamily);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(familyWithNull.Result);
            Assert.IsInstanceOf<BadRequestResult>(existingFamilyResponse.Result);
        }

        [Test]
        public async Task PutFamily_CallPutMethodWithInvalidData_ReceiveBadRequestResult()
        {
            // Act
            var familyWithNull = await familyController.Put(null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(familyWithNull.Result);
        }

        [Test]
        public async Task PutFamily_CallPutMethodWithInvalidData_ReceiveNotFoundResult()
        {
            // Arrange
            var family = new Family { Id = 100, Name = "Tkach", DateCreated = DateTime.Now, DateUpdated = DateTime.Now };

            // Act
            var existingFamilyResponse = await familyController.Put(family);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(existingFamilyResponse.Result);
        }

        [Test]
        public async Task PutFamily_CallPutMethodWithValidData_ReceiveOkResult()
        {
            // Arrange
            var family = budgetContext.Families.FirstOrDefault(f => f.Id == 1);

            // Act
            family.Name = "Test";
            var familyResponse = await familyController.Put(family);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(familyResponse.Result);
        }

        [Test]
        public async Task DeleteFamily_CallDeleteMethodWithInvalidId_ReceiveNotFoundResult()
        {
            // Act
            var existingFamilyResponse = await familyController.Delete(100);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(existingFamilyResponse.Result);
        }

        [Test]
        public async Task DeleteFamily_CallDeleteMethodWithValidId_ReceiveOkResult()
        {
            // Act
            var familyResponse = await familyController.Delete(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(familyResponse.Result);
        }

        [Test]
        public async Task DeleteFamily_CallDeleteMethodWithValidId_ReceiveFamiliesCountMinusOne()
        {
            // Arrange
            var familiesCount = Families().Count;

            // Act
            await familyController.Delete(1);
            var familyResponse = await familyController.Get();

            // Assert
            Assert.AreEqual(((List<Family>)familyResponse).Count, familiesCount - 1);
        }

        private List<Family> Families()
        {
            return new List<Family>
            {
                new Family
                {
                    Name = "Ivanov",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Family
                {
                    Name = "Sidorov",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Family
                {
                    Name = "Tkach",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                }
            };
        }
    }
}