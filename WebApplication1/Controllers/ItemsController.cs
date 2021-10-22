using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        ApplicationContext db;
        IWebHostEnvironment ae;
        public ItemsController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            ae = appEnvironment;
            if (!db.Items.Any())
            {
                db.Items.Add(new Item { ItemName="Item1", Descr = "First item", CategoryId=1});
                db.Items.Add(new Item { ItemName = "Item2", Descr = "Second item", CategoryId = 2  });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            return await db.Items.ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            Item item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Item>> Post(Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            db.Items.Add(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }

        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<Item>> Put(Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (!db.Carts.Any(x => x.Id == item.Id))
            {
                return NotFound();
            }

            db.Update(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> Delete(int id)
        {
            Item item = db.Items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> AddFile(int id,IFormFile uploadedFile)
        {
            Item item = db.Items.FirstOrDefault(x => x.Id == id);
            if (item!=null)
            {

                if (uploadedFile != null)
                {
                    // путь к папке Files
                    string path = "/Files/" + id + uploadedFile.FileName.Split('.')[1];
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(ae.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    item.Img = path;
                    db.Update(item);
                    await db.SaveChangesAsync();
                }
                return Ok(item);
            }
            return BadRequest();
           
        }
    }
}
