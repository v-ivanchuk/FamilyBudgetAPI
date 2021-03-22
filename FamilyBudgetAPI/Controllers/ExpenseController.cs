using FamilyBudgetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
