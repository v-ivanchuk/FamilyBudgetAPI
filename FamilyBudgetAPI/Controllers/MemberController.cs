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

        [HttpGet("[action]")]
        public async Task<IEnumerable<Member>> GetPagination([FromQuery] MemberFilter memberFilter)
        {
            var memberPagination = new Pagination(memberFilter.PageNumber, memberFilter.PageSize, memberFilter.OrderMode);

            var memberQuery = _budgetContext.Members.Include(f => f.Family).AsQueryable();

            memberQuery = AddFiltersToQuery(memberFilter, memberQuery);

            return await memberQuery
                            .OrderBy(memberPagination.OrderMode)
                            .Skip((memberPagination.PageNumber - 1) * memberPagination.PageSize)
                            .Take(memberPagination.PageSize)
                            .ToListAsync();
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

        private IQueryable<Member> AddFiltersToQuery(MemberFilter memberFilter, IQueryable<Member> query)
        {
            if (memberFilter.Id != 0)
            {
                query = query.Where(m => m.Id == memberFilter.Id);
            }

            if (!string.IsNullOrEmpty(memberFilter.Name))
            {
                query = query.Where(m => m.Name == memberFilter.Name);
            }

            if (!string.IsNullOrEmpty(memberFilter.Surname))
            {
                query = query.Where(m => m.Surname == memberFilter.Surname);
            }

            if (!string.IsNullOrEmpty(memberFilter.Email))
            {
                query = query.Where(m => m.Email == memberFilter.Email);
            }

            if (!string.IsNullOrEmpty(memberFilter.PhoneNumber))
            {
                query = query.Where(m => m.PhoneNumber == memberFilter.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(memberFilter.Address))
            {
                query = query.Where(m => m.Address == memberFilter.Address);
            }

            if (!string.IsNullOrEmpty(memberFilter.City))
            {
                query = query.Where(m => m.City == memberFilter.City);
            }

            if (memberFilter.FamilyId != 0)
            {
                query = query.Where(m => m.FamilyId == memberFilter.FamilyId);
            }

            if (memberFilter.DateCreated != default(DateTime))
            {
                query = query.Where(m => m.DateCreated == memberFilter.DateCreated);
            }

            if (memberFilter.DateUpdated != default(DateTime))
            {
                query = query.Where(m => m.DateUpdated == memberFilter.DateUpdated);
            }

            if (memberFilter.Family != null)
            {
                if (memberFilter.Family.Id != 0)
                {
                    query = query.Where(m => m.Family.Id == memberFilter.Family.Id);
                }

                if (!string.IsNullOrEmpty(memberFilter.Family.Name))
                {
                    query = query.Where(m => m.Family.Name == memberFilter.Family.Name);
                }

                if (memberFilter.Family.DateCreated != default(DateTime))
                {
                    query = query.Where(m => m.Family.DateCreated == memberFilter.Family.DateCreated);
                }

                if (memberFilter.Family.DateUpdated != default(DateTime))
                {
                    query = query.Where(m => m.Family.DateUpdated == memberFilter.Family.DateUpdated);
                }
            }

            return query;
        }
    }
}
