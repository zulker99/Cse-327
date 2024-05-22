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


        public async Task<IActionResult> SeeBooks(string searchTerm)
        {
            var booksQuery = from book in _webDB.Books
                             join user in _webDB.Users on book.UserID equals user.ID
                             where book.Status == "Active"
                             select new _ShowBook
                             {
                                 ID = book.ID,
                                 Title = book.Title,
                                 Course = book.Course,
                                 FileUrl = book.FileUrl,
                                 DateTime = book.DateTime,
                                 UserName = user.Name,
                                 Status = book.Status,
                                 UserID = book.UserID

                             };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchTerm) || b.Course.Contains(searchTerm));
            }

            var books = await booksQuery.ToListAsync();
            return View(books);
        }



        public IActionResult SignUp()
        {
            return View();
        }

        
        public IActionResult LogIn()
        {
            string UserID = HttpContext.Request.Cookies["UserID"];
            string AdminID = HttpContext.Request.Cookies["AdminID"];

            if (UserID != null || AdminID != null)
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
                                                UserName = user.Name, // Extracting the username from the Users table
                                                Status = post.Status,
                                                Comments = (from comment in _webDB.Comments
                                                            join commentUser in _webDB.Users on comment.UserID equals commentUser.ID
                                                            where comment.PostID == post.ID && comment.Status == "Active"
                                                            select new CommentViewModel
                                                            {
                                                                ID = comment.ID,
                                                                UserID = comment.UserID,
                                                                PostID = comment.PostID,
                                                                Content = comment.Content,
                                                                Status = comment.Status,
                                                                DateTime = comment.DateTime,
                                                                UserName = commentUser.Name // Extracting the username from the Users table
                                                            }).OrderBy(c=>c.DateTime).ToList()
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
