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
    public class CartController : ControllerBase
    {
        ApplicationContext db;
        public CartController(ApplicationContext context)
        {
            db = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> Get()
        {
            return await db.Carts.ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> Get(int id)
        {
            Cart cart = await db.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (cart == null)
                return NotFound();
            return new ObjectResult(cart);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Cart>> Post(Cart cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }

            db.Carts.Add(cart);
            await db.SaveChangesAsync();
            return Ok(cart);
        }

        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<Cart>> Put(Cart cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            if (!db.Carts.Any(x => x.Id == cart.Id))
            {
                return NotFound();
            }

            db.Update(cart);
            await db.SaveChangesAsync();
            return Ok(cart);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> Delete(int id)
        {
            Cart cart = db.Carts.FirstOrDefault(x => x.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            db.Carts.Remove(cart);
            await db.SaveChangesAsync();
            return Ok(cart);
        }
    }
}
