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
    public class FamilyController : ControllerBase
    {
        private readonly BudgetContext _budgetContext;

        public FamilyController(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Family>> Get()
        {
            return await _budgetContext.Families.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Family>> GetPagination([FromQuery] FamilyFilter family)
        {
            var familyParams = new Pagination(family.PageNumber, family.PageSize, family.OrderMode);

            var filterResult =  _budgetContext.Families.Any(f => f.Id == family.Id
                                                         || f.Name == family.Name
                                                         || f.DateCreated == family.DateCreated
                                                         || f.DateUpdated == family.DateUpdated);

            if (filterResult)
            {
                return await _budgetContext.Families
                                .Where(f => f.Id == family.Id
                                         || f.Name == family.Name
                                         || f.DateCreated == family.DateCreated
                                         || f.DateUpdated == family.DateUpdated)
                                .OrderBy(familyParams.OrderMode)
                                .Skip((familyParams.PageNumber - 1) * familyParams.PageSize)
                                .Take(familyParams.PageSize)
                                .ToListAsync();
            }
            else
            {
                return await _budgetContext.Families
                                .OrderBy(familyParams.OrderMode)
                                .Skip((familyParams.PageNumber - 1) * familyParams.PageSize)
                                .Take(familyParams.PageSize)
                                .ToListAsync();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Family>> Get(int id)
        {
            Family family = await _budgetContext.Families.FirstOrDefaultAsync(m => m.Id == id);

            if (family == null)
            {
                return NotFound();
            }

            return new ObjectResult(family);
        }

        [HttpPost]
        public async Task<ActionResult<Family>> Post(Family family)
        {
            if (family == null || _budgetContext.Families.Any(m => m.Id == family.Id))
            {
                return BadRequest();
            }

            family.DateCreated = DateTime.Now;
            family.DateUpdated = DateTime.Now;

            _budgetContext.Families.Add(family);
            await _budgetContext.SaveChangesAsync();

            family = _budgetContext.Families
                .FirstOrDefault(m => m.Name == family.Name
                && m.DateCreated == family.DateCreated
                && m.DateUpdated == family.DateUpdated);
            return Ok(family);
        }

        [HttpPut]
        public async Task<ActionResult<Family>> Put(Family family)
        {
            if (family == null)
            {
                return BadRequest();
            }

            if (!_budgetContext.Families.Any(m => m.Id == family.Id))
            {
                return NotFound();
            }

            if(family.DateUpdated == default(DateTime))
            {
                family.DateCreated = _budgetContext.Families
                                .FirstOrDefault(m => m.Id == family.Id)
                                .DateCreated;
            }

            family.DateUpdated = DateTime.Now;

            _budgetContext.Families.Update(family);
            await _budgetContext.SaveChangesAsync();

            family = _budgetContext.Families
                .FirstOrDefault(m => m.Id == family.Id);
            return Ok(family);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Family>> Delete(int id)
        {
            Family family = await _budgetContext.Families.FirstOrDefaultAsync(m => m.Id == id);

            if (family == null)
            {
                return NotFound();
            }

            _budgetContext.Families.Remove(family);
            await _budgetContext.SaveChangesAsync();
            return Ok(family);
        }
    }
}
