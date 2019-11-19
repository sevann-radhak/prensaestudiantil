using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationCategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public PublicationCategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PublicationCategories
        [HttpGet]
        public IEnumerable<PublicationCategory> GetPublicationCategories()
        {
            return _context.PublicationCategories.OrderBy(pc => pc.Name);
        }

        //// GET: api/PublicationCategories/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPublicationCategory([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var publicationCategory = await _context.PublicationCategories.FindAsync(id);

        //    if (publicationCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(publicationCategory);
        //}

        //// PUT: api/PublicationCategories/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPublicationCategory([FromRoute] int id, [FromBody] PublicationCategory publicationCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != publicationCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(publicationCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PublicationCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/PublicationCategories
        //[HttpPost]
        //public async Task<IActionResult> PostPublicationCategory([FromBody] PublicationCategory publicationCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.PublicationCategories.Add(publicationCategory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPublicationCategory", new { id = publicationCategory.Id }, publicationCategory);
        //}

        //// DELETE: api/PublicationCategories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePublicationCategory([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var publicationCategory = await _context.PublicationCategories.FindAsync(id);
        //    if (publicationCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PublicationCategories.Remove(publicationCategory);
        //    await _context.SaveChangesAsync();

        //    return Ok(publicationCategory);
        //}

        //private bool PublicationCategoryExists(int id)
        //{
        //    return _context.PublicationCategories.Any(e => e.Id == id);
        //}
    }
}