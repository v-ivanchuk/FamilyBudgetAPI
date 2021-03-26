using FamilyBudgetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace FamilyBudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;

        public ExpenseController(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Expense>> Get()
        {
            try
            {
                return await _budgetContext.Expenses
                                .Include(ex => ex.Member)
                                .ToListAsync();
            }
            catch
            {
                return null;
            }

        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Expense>> GetPagination([FromQuery] ExpenseFilter expenseFilter)
        {
            var expensePagination = new Pagination(expenseFilter.PageNumber, expenseFilter.PageSize, expenseFilter.OrderMode);

            var expenseQuery = _budgetContext.Expenses.Include(m => m.Member).AsQueryable();

            expenseQuery = AddFiltersToQuery(expenseFilter, expenseQuery);

            return await expenseQuery
                            .OrderBy(expensePagination.OrderMode)
                            .Skip((expensePagination.PageNumber - 1) * expensePagination.PageSize)
                            .Take(expensePagination.PageSize)
                            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> Get(int id)
        {
            Expense expense = await _budgetContext.Expenses
                                        .Include(ex => ex.Member)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return new ObjectResult(expense);
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Post(Expense expense)
        {
            if (expense == null 
                || _budgetContext.Expenses.Any(ex => ex.Id == expense.Id)
                || !_budgetContext.Members.Any(m => m.Id == expense.MemberId))
            {
                return BadRequest();
            }

            expense.DateCreated = DateTime.Now;
            expense.DateUpdated = DateTime.Now;

            _budgetContext.Expenses.Add(expense);
            await _budgetContext.SaveChangesAsync();

            expense = _budgetContext.Expenses
                .FirstOrDefault(ex => ex.DateCreated == expense.DateCreated
                && ex.DateUpdated == expense.DateUpdated
                && ex.Description == expense.Description);
            return Ok(expense);
        }

        [HttpPut]
        public async Task<ActionResult<Expense>> Put(Expense expense)
        {
            if (expense == null
                || !_budgetContext.Members.Any(m => m.Id == expense.MemberId))
            {
                return BadRequest();
            }

            if (!_budgetContext.Expenses.Any(m => m.Id == expense.Id))
            {
                return NotFound();
            }

            expense.DateUpdated = DateTime.Now;

            _budgetContext.Expenses.Update(expense);
            await _budgetContext.SaveChangesAsync();

            expense = _budgetContext.Expenses
                .FirstOrDefault(ex => ex.Id == expense.Id);
            return Ok(expense);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> Delete(int id)
        {
            Expense expense = await _budgetContext.Expenses.FirstOrDefaultAsync(m => m.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            _budgetContext.Expenses.Remove(expense);
            await _budgetContext.SaveChangesAsync();
            return Ok(expense);
        }

        private IQueryable<Expense> AddFiltersToQuery(ExpenseFilter expenseFilter, IQueryable<Expense> query)
        {
            if (expenseFilter.Id != 0)
            {
                query = query.Where(ex => ex.Id == expenseFilter.Id);
            }

            if (!string.IsNullOrEmpty(expenseFilter.Description))
            {
                query = query.Where(ex => ex.Description == expenseFilter.Description);
            }

            if (expenseFilter?.Value != 0m)
            {
                query = query.Where(ex => ex.Value == expenseFilter.Value);
            }

            if (expenseFilter.MemberId != 0)
            {
                query = query.Where(ex => ex.MemberId == expenseFilter.MemberId);
            }

            if (expenseFilter.ExpenseDate != default(DateTime))
            {
                query = query.Where(ex => ex.ExpenseDate == expenseFilter.ExpenseDate);
            }

            if (expenseFilter.DateCreated != default(DateTime))
            {
                query = query.Where(ex => ex.DateCreated == expenseFilter.DateCreated);
            }

            if (expenseFilter.DateUpdated != default(DateTime))
            {
                query = query.Where(ex => ex.DateUpdated == expenseFilter.DateUpdated);
            }

            if(expenseFilter.Member != null)
            {
                if (expenseFilter.Member.Id != 0)
                {
                    query = query.Where(ex => ex.Member.Id == expenseFilter.Member.Id);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.Name))
                {
                    query = query.Where(ex => ex.Member.Name == expenseFilter.Member.Name);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.Surname))
                {
                    query = query.Where(ex => ex.Member.Surname == expenseFilter.Member.Surname);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.Email))
                {
                    query = query.Where(ex => ex.Member.Email == expenseFilter.Member.Email);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.PhoneNumber))
                {
                    query = query.Where(ex => ex.Member.PhoneNumber == expenseFilter.Member.PhoneNumber);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.Address))
                {
                    query = query.Where(ex => ex.Member.Address == expenseFilter.Member.Address);
                }

                if (!string.IsNullOrEmpty(expenseFilter.Member.City))
                {
                    query = query.Where(ex => ex.Member.City == expenseFilter.Member.City);
                }

                if (expenseFilter.Member.DateCreated != default(DateTime))
                {
                    query = query.Where(ex => ex.Member.DateCreated == expenseFilter.Member.DateCreated);
                }

                if (expenseFilter.Member.DateUpdated != default(DateTime))
                {
                    query = query.Where(ex => ex.Member.DateUpdated == expenseFilter.Member.DateUpdated);
                }
            }

            return query;
        }
    }
}
