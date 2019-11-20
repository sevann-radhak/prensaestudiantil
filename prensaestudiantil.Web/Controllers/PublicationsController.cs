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
        private readonly DataContext _dataContext;
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
            _dataContext = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
        }

        // GET: Publications
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Publications
                .Include(p => p.User)
                .Include(p => p.PublicationCategory)
                .OrderByDescending(p => p.Date)
                .Take(500)
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEditImages(int? id)
        {
            if (id == null || !PublicationExists(id.Value))
            {
                return NotFound();
            }

            var publication = await _dataContext.Publications
                .Include(p => p.PublicationImages)
                .Where(p => p.Id == id.Value)
                .FirstOrDefaultAsync();

            PublicationImageViewModel model = new PublicationImageViewModel
            {
                PublicationId = id.Value,
                PublicationTitle = publication.Title,
                PublicationImages = publication.PublicationImages == null
                    ? null
                    : publication.PublicationImages.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditImages(PublicationImageViewModel model)
        {
            // TODO: fix error when refresh page after adding an image (add twice)
            var publication = await _dataContext.Publications
                .FindAsync(model.PublicationId);

            if (model.ImageFile != null)
            {
                _dataContext.PublicationImages.Add(new PublicationImage
                {
                    Description = model.Description,
                    ImageUrl = await _imageHelper.UploadImageAsync(model.ImageFile),
                    Publication = publication
                });

                try
                {
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }

            }

            model.PublicationImages = _dataContext.PublicationImages
                .Where(pi => pi.Publication == publication).ToList();

            return View(model);
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
        public async Task<IActionResult> Create(PublicationViewModel model, bool addImages)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found. Call support.");
                    model.PublicationCategories = _combosHelper.GetComboPublicationCategories();

                    return View(model);
                }
                if (model.ImageFile != null)
                {
                    model.ImageUrl = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                model.Date = DateTime.Now.ToUniversalTime();
                model.UserId = user.Id;

                _dataContext.Publications.Add(await _converterHelper.ToPublicationAsync(model, true));

                try
                {
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
                    return View(model);
                }

                TempData["Success"] = "Publication created successfully!";

                // TODO: fix the publicationId recover method... Need create and get it back 
                var publication = await _dataContext.Publications
                    .Include(p => p.User)
                    .LastOrDefaultAsync();

                return addImages
                    ? RedirectToAction($"{nameof(AddEditImages)}/{publication.Id}")
                    : RedirectToAction(nameof(Index));
            }

            model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
            return View(model);
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _dataContext.Publications.FindAsync(id.Value);

            if (publication == null)
            {
                return NotFound();
            }

            _dataContext.Publications.Remove(publication);

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if ((ex.InnerException != null
                    && ex.InnerException.InnerException != null
                    && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    ||
                    (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE"))
                   )
                {
                    TempData["Error"] = "Can not delete this record because has related records";
                }
                else
                {
                    TempData["Error"] = $"Can not delete this record. Call support! {ex}";
                }
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Publications deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletePublicationImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _dataContext.PublicationImages
                .Include(pi => pi.Publication)
                .Where(pi => pi.Id == id.Value)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            _dataContext.PublicationImages.Remove(model);

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }

            return RedirectToAction($"{nameof(AddEditImages)}/{model.Publication.Id}");
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _dataContext.Publications
                .Include(p => p.User)
                .Include(p => p.PublicationCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !PublicationExists(id.Value))
            {
                return NotFound();
            }

            var publication = await _dataContext.Publications
               .Include(p => p.PublicationCategory)
               .Include(p => p.PublicationImages)
               .Include(p => p.User)
               .Where(p => p.Id == id).FirstOrDefaultAsync();

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
                    if (model.ImageFile != null)
                    {
                        model.ImageUrl = await _imageHelper.UploadImageAsync(model.ImageFile);
                    }

                    model.LastUpdate = DateTime.Now;
                    _dataContext.Update(await _converterHelper.ToPublicationAsync(model, false));
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    model.PublicationCategories = _combosHelper.GetComboPublicationCategories();
                    return View(model);
                }
            }

            TempData["Success"] = "Publication updated successfully!";
            return RedirectToAction($"{nameof(Details)}/{model.Id}");
        }

        // POST: Publications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var publication = await _dataContext.Publications.FindAsync(id);
        //    _dataContext.Publications.Remove(publication);
        //    await _dataContext.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PublicationExists(int id)
        {
            return _dataContext.Publications.Any(e => e.Id == id);
        }
    }
}
