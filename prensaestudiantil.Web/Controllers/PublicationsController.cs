using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Helpers;
using prensaestudiantil.Web.Models;

namespace prensaestudiantil.Web.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IUserHelper _userHelper;

        public PublicationsController(
            DataContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper,
            IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
        }

        // GET: Publications
        public async Task<IActionResult> Index()
        {
            return View(await _context.Publications.ToListAsync());
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // GET: Publications/Create
        public IActionResult Create()
        {
            PublicationViewModel model = new PublicationViewModel
            {
                PublicationCategories = _combosHelper.GetComboPublicationCategories()
            };

            return View(model);
        }

        // POST: Publications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found. Call support.");
                    model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
                    return View(model);
                }
                model.Date = DateTime.Now.ToUniversalTime();
                model.ImageUrl = await _imageHelper.UploadImageAsync(model.ImageFile);
                model.UserId = user.Id;

                _context.Add(await _converterHelper.ToPublicationAsync(model, true));
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
            return View(model);
        }

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .Include(p => p.PublicationCategory)
                .Include(p => p.PublicationImages)
                .Include(p => p.User)
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (publication == null)
            {
                return NotFound();
            }

            PublicationViewModel model = _converterHelper.ToPublicationViewModel(publication, false);
            model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
            return View(model);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PublicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.LastUpdate = DateTime.Now.ToUniversalTime();
                    _context.Update(await _converterHelper.ToPublicationAsync(model, false));
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
                    return View(model);
                }
            }

            TempData["Success"] = "Publications updated successfully!";

            return RedirectToAction($"Details/{model.Id}");
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
