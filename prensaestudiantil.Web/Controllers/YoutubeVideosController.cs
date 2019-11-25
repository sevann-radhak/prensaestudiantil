using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data.Entities;
using prensaestudiantil.Web.Data.Repositories;
using prensaestudiantil.Web.Helpers;

namespace prensaestudiantil.Web.Controllers
{
    [Authorize]
    public class YoutubeVideosController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IYoutubeVideosRepository _youtubeVideosRepository;

        public YoutubeVideosController(
            IUserHelper userHelper,
            IYoutubeVideosRepository youtubeVideosRepository)
        {
            _userHelper = userHelper;
            _youtubeVideosRepository = youtubeVideosRepository;
        }

        // GET: YoutubeVideos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _youtubeVideosRepository.GetAll().Take(200).ToListAsync());
        }

        // GET: Publications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YoutubeVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,URL,Name")] YoutubeVideo youtubeVideo)
        {
            if (ModelState.IsValid)
            {
                //_dataContext.Add(youtubeVideo);
                //await _dataContext.SaveChangesAsync();
                await _youtubeVideosRepository.CreateAsync(youtubeVideo);
                TempData["Success"] = "Video cargado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
            return View(youtubeVideo);
        }

        // GET: YoutubeVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var youtubeVideo = await _youtubeVideosRepository.GetByIdAsync(id.Value);
            if (youtubeVideo == null)
            {
                return NotFound();
            }
            return View(youtubeVideo);
        }

        // POST: YoutubeVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,URL,Name")] YoutubeVideo youtubeVideo)
        {
            if (id != youtubeVideo.Id)
            {
                return NotFound();
            }
            if (!await _youtubeVideosRepository.ExistAsync(youtubeVideo.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_dataContext.Update(youtubeVideo);
                    //await _dataContext.SaveChangesAsync();
                    await _youtubeVideosRepository.UpdateAsync(youtubeVideo);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(youtubeVideo);
                }

                TempData["Success"] = "Video updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(youtubeVideo);
        }

        // GET: YoutubeVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var youtubeVideo = await _youtubeVideosRepository.GetByIdAsync(id.Value);
            if (youtubeVideo == null)
            {
                return NotFound();
            }

            if (!await UserHasPermissionsOnVideoAsync(youtubeVideo))
            {
                return RedirectToAction("NotAuthorized", "Account");
            }

            try
            {
                await _youtubeVideosRepository.DeleteAsync(youtubeVideo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"No se pudo eliminar el registro. Comuníquese con el administrador!: {ex}";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Video eliminado exitosamente!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserHasPermissionsOnVideoAsync(YoutubeVideo video)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            return !User.IsInRole("Manager") && video.User != user
                ? false
                : true;
        }
    }
}
