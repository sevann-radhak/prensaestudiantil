using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Data.Repositories;

namespace prensaestudiantil.Web.Controllers
{
    public class PublicationCategoriesController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IPublicationCategoryRepository _publicationCategoryRepository;

        public PublicationCategoriesController(
            DataContext dataContext,
            IPublicationCategoryRepository publicationCategoryRepository)
        {
            _dataContext = dataContext;
            _publicationCategoryRepository = publicationCategoryRepository;
        }

        // GET: PublicationCategories
        public IActionResult Index()
        {
            return View(_publicationCategoryRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: PublicationCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (! await _publicationCategoryRepository.ExistAsync(id.Value))
            {
                return NotFound();
            }

            var publicationCategory = await _publicationCategoryRepository.GetByIdAsync(id.Value);

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
        public async Task<IActionResult> Create(PublicationCategory model)
        {
            if (ModelState.IsValid)
            {
                await _publicationCategoryRepository.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PublicationCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicationCategory = await _publicationCategoryRepository.GetByIdAsync(id.Value);
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
        public async Task<IActionResult> Edit(int id, PublicationCategory model)
        {
            if (id != model.Id || !await _publicationCategoryRepository.ExistAsync(model.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _publicationCategoryRepository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Fatal error, intente nuevamente o comuníquese con " +
                        $"el administrador. Error: {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PublicationCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!await _publicationCategoryRepository.ExistAsync(id.Value))
            {
                return NotFound();
            }

            var model = await _publicationCategoryRepository.GetByIdAsync(id.Value);
            try
            {
                await _publicationCategoryRepository.DeleteAsync(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["Error"] = $"Fatal error, intente nuevamente o comuníquese con " +
                    $"el administrador. Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
