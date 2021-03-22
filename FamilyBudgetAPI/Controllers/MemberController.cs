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
    public class MemberController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;

        public MemberController(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Member>> Get()
        {
            return await _budgetContext.Members.Include(m => m.Family).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> Get(int id)
        {
            Member member = await _budgetContext.Members.FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            return new ObjectResult(member);
        }

        [HttpPost]
        public async Task<ActionResult<Member>> Post(Member member)
        {
            if (member == null 
                || _budgetContext.Members.Any(m => m.Id == member.Id)
                || !_budgetContext.Families.Any(f => f.Id == member.FamilyId))
            {
                return BadRequest();
            }

            member.DateCreated = DateTime.Now;
            member.DateUpdated = DateTime.Now;

            _budgetContext.Members.Add(member);
            await _budgetContext.SaveChangesAsync();

            member = await _budgetContext.Members
                .FirstOrDefaultAsync(m => m.Name == member.Name
                && m.DateCreated == member.DateCreated
                && m.DateUpdated == member.DateUpdated);
            return Ok(member);
        }

        [HttpPut]
        public async Task<ActionResult<Member>> Put(Member member)
        {
            if (member == null
                || !_budgetContext.Families.Any(f => f.Id == member.FamilyId))
            {
                return BadRequest();
            }

            if (!_budgetContext.Members.Any(m => m.Id == member.Id))
            {
                return NotFound();
            }

            member.DateUpdated = DateTime.Now;

            _budgetContext.Members.Update(member);
            await _budgetContext.SaveChangesAsync();

            member = await _budgetContext.Members
                .FirstOrDefaultAsync(m => m.Id == member.Id);
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> Delete(int id)
        {
            Member member = await _budgetContext.Members.FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            _budgetContext.Members.Remove(member);
            await _budgetContext.SaveChangesAsync();
            return Ok(member);
        }
    }
}
