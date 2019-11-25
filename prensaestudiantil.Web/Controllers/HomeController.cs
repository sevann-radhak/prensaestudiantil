using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prensaestudiantil.Web.Data;
using prensaestudiantil.Web.Models;

namespace prensaestudiantil.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;

        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(new MainIndexViewModel
            {
                PublicationImages = _dataContext.PublicationImages.Take(10).ToList(),
                Publications = await _dataContext.Publications
                    .Include(p => p.PublicationCategory)
                    .OrderByDescending(p => p.Date)
                    .Take(12).ToListAsync(),
                OpinionPublications = await _dataContext.Publications
                    .Include(p => p.PublicationCategory)
                    .Where(p => p.PublicationCategory.Name == "Opinión")
                    .OrderByDescending(p => p.Date)
                    .Take(3).ToListAsync(),
                YoutubeVideos = await _dataContext.YoutubeVideos
                .OrderByDescending(y => y.Id)
                .Take(8)
                .ToListAsync()
            });
        }

        public async Task<IActionResult> Search()
        {
            return View(await _dataContext.Publications
                .Include(p => p.User)
                .Include(p => p.PublicationCategory)
                .OrderByDescending(p => p.Date)
                .Take(200)
                .ToListAsync());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

    }
}
