using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ApplicationContext db;
        public CategoriesController(ApplicationContext context)
        {
            db = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return await db.Categories.ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            Category category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            return new ObjectResult(category);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return Ok(category);
        }

        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<Category>> Put(Category category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            if (!db.Categories.Any(x => x.Id == category.Id))
            {
                return NotFound();
            }

            db.Update(category);
            await db.SaveChangesAsync();
            return Ok(category);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            Category category = db.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Ok(category);
        }
    }
}
