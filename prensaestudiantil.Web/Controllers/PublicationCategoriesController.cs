using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;

namespace prensaestudiantil.Web.Controllers
{
    public class PublicationCategoriesController : Controller
    {
        private readonly DataContext _context;

        public PublicationCategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: PublicationCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.PublicationCategories.ToListAsync());
        }

        // GET: PublicationCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationCategory = await _context.PublicationCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicationCategory == null)
            {
                return NotFound();
            }

            return View(publicationCategory);
        }

        // GET: PublicationCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublicationCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PublicationCategory publicationCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publicationCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publicationCategory);
        }

        // GET: PublicationCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationCategory = await _context.PublicationCategories.FindAsync(id);
            if (publicationCategory == null)
            {
                return NotFound();
            }
            return View(publicationCategory);
        }

        // POST: PublicationCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PublicationCategory publicationCategory)
        {
            if (id != publicationCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicationCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationCategoryExists(publicationCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publicationCategory);
        }

        // GET: PublicationCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationCategory = await _context.PublicationCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publicationCategory == null)
            {
                return NotFound();
            }

            return View(publicationCategory);
        }

        // POST: PublicationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicationCategory = await _context.PublicationCategories.FindAsync(id);
            _context.PublicationCategories.Remove(publicationCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationCategoryExists(int id)
        {
            return _context.PublicationCategories.Any(e => e.Id == id);
        }
    }
}
