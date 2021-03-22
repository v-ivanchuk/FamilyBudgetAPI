using FamilyBudgetAPI.Models;
using System;
using System.Linq;

namespace FamilyBudgetAPI
{
    public class SampleData
    {
        public static void Initialize(BudgetContext context)
        {
            if (context.Families.Any())
            {
                return;
            }
            if (context.Members.Any())
            {
                return;
            }
            if (context.Expenses.Any())
            {
                return;
            }

            context.Families.AddRange(
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
            );

            context.SaveChanges();

            context.Members.AddRange(
                new Member
                {
                    Name = "Den",
                    Surname = "Ivanov",
                    Email = "ivanov@ukr.net",
                    PhoneNumber = "0981234567",
                    Address = "st. Franka 5",
                    City = "Kyiv",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Family = context.Families.FirstOrDefault(f => f.Name == "Ivanov")
                },
                new Member
                {
                    Name = "Donald",
                    Surname = "Sidorov",
                    Email = "sidorov@gmail.com",
                    PhoneNumber = "0967654321",
                    Address = "st. Shevchenko 1",
                    City = "Vinnytsia",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Family = context.Families.FirstOrDefault(f => f.Name == "Sidorov")
                },
                new Member
                {
                    Name = "Joe",
                    Surname = "Tkach",
                    Email = "tkach@.com",
                    PhoneNumber = "0974563217",
                    Address = "st. Murnoho 1",
                    City = "Zhitomir",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Family = context.Families.FirstOrDefault(f => f.Name == "Tkach")
                }
            );

            context.SaveChanges();

            context.Expenses.AddRange(
                new Expense
                {
                    Value = 50m,
                    Description = "Ice-cream",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Den")
                },
                new Expense
                {
                    Value = 15m,
                    Description = "Mars",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Den")
                },
                new Expense
                {
                    Value = 10m,
                    Description = "Chocolate",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Donald")
                },
                new Expense
                {
                    Value = 20m,
                    Description = "Lion",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Donald")
                },
                new Expense
                {
                    Value = 113m,
                    Description = "Burger",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Joe")
                },
                new Expense
                {
                    Value = 23m,
                    Description = "Waffles",
                    ExpenseDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Member = context.Members.FirstOrDefault(m => m.Name == "Joe")
                }
            );

            context.SaveChanges();
        }
    }
}
