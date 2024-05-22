using AcademicAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace AcademicAssistant.Controllers
{
    public class UserController : Controller
    {
        private static readonly Random random = new Random();
        WebDbContext _webDB;

        public UserController(WebDbContext webDB)
        {
            _webDB = webDB;
        }

        public async Task<IActionResult> UserDashboard()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> UpdatePost(string id, string postTitle, string postContent, string postImage)
        {

            var post = await _webDB.Posts.Where(c => c.ID == id).FirstOrDefaultAsync();
            if (post != null)
            {
                if (!string.IsNullOrEmpty(postTitle) || !string.IsNullOrEmpty(postContent))
                {
                    post.Title = postTitle;
                    post.Content = postContent;
                    post.ImgUrl = postImage;

                    TempData["PopupScript"] = "<script>alert('Your Post Has Been Updated');</script>";
                    await _webDB.SaveChangesAsync();

                }
                else
                {
                    TempData["PopupScript"] = "<script>alert('Insert Title and Content appropriately!');</script>";
                }
            }


            return RedirectToAction("EditPost", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string postTitle, string postContent, string postImage)
        {

            string userID = HttpContext.Request.Cookies["UserID"];
            if (userID == null || userID == "")

            Random random = new Random();
            if (!string.IsNullOrEmpty(postTitle) || !string.IsNullOrEmpty(postContent)  ) {
                _webDB.Posts.Add(new Models.Post
                {
                    ID = random.Next(10000, 99999) + "-" + random.Next(10000, 99999) + "-" + random.Next(10000, 99999),
                    Title = postTitle,
                    Content = postContent,
                    ImgUrl = postImage,
                    DateTime = DateTime.Now,
                    UserID = userID,
                    Status = "Active"
                }) ;
                await _webDB.SaveChangesAsync();
                TempData["PopupScript"] = "<script>alert('Your post was created!');</script>";
            }
            else
            {
                TempData["PopupScript"] = "<script>alert('Insert Title and Content appropriately!');</script>";
            }

            return RedirectToAction("MakePost");
        }

        public async Task<IActionResult> MakePost()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUser(string ID,
                                  string FullName,
                                  string PhoneNumber,
                                  string Email)
        {


            var user = await _webDB.Users.Where(c => c.ID == ID).FirstOrDefaultAsync();
            
            user.Name = FullName;
            user.PhoneNumber = PhoneNumber;
            user.Email = Email;

            Response.Cookies.Append("Name", FullName);
            Response.Cookies.Append("Email", Email);
            Response.Cookies.Append("PhoneNumber", PhoneNumber);

            await _webDB.SaveChangesAsync();
           
            return RedirectToAction("UserDashboard");


        }


        [HttpPost]
        public async Task<IActionResult> LoginUser(
                string emailInput, string passwordInput
            )
        {
            var user = await _webDB.Users.Where(c=>c.Email == emailInput && c.Password == passwordInput).FirstOrDefaultAsync();
            string script;
            if (user == null)
            {
                script = "<script>alert('Invalid Credentials');</script>";
                TempData["PopupScript"] = script;
                return RedirectToAction("Login", "Home");
            }


            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(10);
            
            Response.Cookies.Append("UserID", "", options);
            Response.Cookies.Append("UserID", user.ID, options);

            Response.Cookies.Append("Email", user.Email, options);
            Response.Cookies.Append("PhoneNumber", user.PhoneNumber, options);
            Response.Cookies.Append("Name", user.Name, options);



            return RedirectToAction("Index", "Home");
        }
        public IActionResult SignOut()
        {
            Response.Cookies.Delete("UserID");
            Response.Cookies.Delete("Name");
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("PhoneNumber");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SignUpUser(string nameInput, string emailInput,string phone,  string passwordInput)
        {
            var user = await _webDB.Users.Where(c => c.Email == emailInput).FirstOrDefaultAsync();
            string script = "";
            if (user != null)
            {
                script = "<script>alert('Email is already in use. Please try a different email.');</script>";
                TempData["PopupScript"] = script;
                return RedirectToAction("SignUp", "Home");
            }

            if (passwordInput.Length < 8)
            {
                script = "<script>alert('Password must be atleast 8 characters long');</script>";
                TempData["PopupScript"] = script;
                return RedirectToAction("SignUp", "Home");
            }

            if (!phone.StartsWith("01"))
            {
                script = "<script>alert('Invalid Phone Number');</script>";
                TempData["PopupScript"] = script;
                return RedirectToAction("SignUp", "Home");
            }

            _webDB.Users.Add(new Models.User
            {
                ID = random.Next(10000, 99999) + "-" + random.Next(10000, 99999) + "-" + random.Next(10000, 99999),
                Name = nameInput,        
                Email = emailInput,        
                Password = passwordInput ,        
                PhoneNumber = phone,        
                Status = "Active",  
                AccountType = "User",
            });

            await _webDB.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        
    }
}
