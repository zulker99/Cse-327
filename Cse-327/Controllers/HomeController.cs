using AcademicAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AcademicAssistant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        WebDbContext _webDB;

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            string UserID = HttpContext.Request.Cookies["UserID"];

            if (UserID != null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public HomeController(ILogger<HomeController> logger, WebDbContext webDB)
        {
            _logger = logger;
            _webDB = webDB;
        }

        public async Task<IActionResult> Index()
        {
            var postsWithUserNames = await (from post in _webDB.Posts
                                            join user in _webDB.Users on post.UserID equals user.ID
                                            where post.Status == "Active"
                                            select new _ShowPost
                                            {
                                                ID = post.ID,
                                                Title = post.Title,
                                                Content = post.Content,
                                                ImgUrl = post.ImgUrl,
                                                DateTime = post.DateTime,
                                                UserID = post.UserID,
                                                UserName = user.Name,
                                                Status = post.Status
                                            }).ToListAsync();

            ViewBag.posts = postsWithUserNames;

            return View();
        }
        public IActionResult AboutUs()
        {
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
    }
}
