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