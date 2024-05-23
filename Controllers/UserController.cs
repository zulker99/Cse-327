using AcademicAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicAssistant.Controllers
{
    /// <summary>
    /// Controller to manage user-related actions such as creating, updating, and deleting books, posts, and user information.
    /// </summary>
    public class UserController : Controller
    {
        private static readonly Random random = new Random();
        private readonly WebDbContext _webDB;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="webDB">The database context to interact with the database.</param>
        public UserController(WebDbContext webDB)
        {
            _webDB = webDB;
        }

        /// <summary>
        /// Shows the respective dashboard for each role.
        /// </summary>
        /// <returns>The dashboard view.</returns>
        public async Task<IActionResult> UserDashboard()
        {
            return View();
        }

        /// <summary>
        /// Displays the view to add books for a user.
        /// </summary>
        /// <returns>The AddBooks view.</returns>
        public async Task<IActionResult> AddBooks()
        {
            return View();
        }

        /// <summary>
        /// Deletes a book by marking it as inactive.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>Redirects to the Home index action.</returns>
        public async Task<IActionResult> DeleteBook(string id)
        {
            var book = await _webDB.Books.Where(c => c.ID == id).FirstOrDefaultAsync();
            if (book != null)
            {
                book.Status = "Inactive";
                await _webDB.SaveChangesAsync();
                TempData["PopupScript"] = "<script>alert('Book Deleted');</script>";
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Creates a new book for the user.
        /// </summary>
        /// <param name="Title">The title of the book.</param>
        /// <param name="Course">The course associated with the book.</param>
        /// <param name="File">The file URL of the book.</param>
        /// <returns>Redirects to the AddBooks view.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook(string Title, string Course, string File)
        {
            string userID = HttpContext.Request.Cookies["UserID"] ?? HttpContext.Request.Cookies["AdminID"];
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(File))
            {
                _webDB.Books.Add(new Book
                {
                    ID = $"{random.Next(10000, 99999)}-{random.Next(10000, 99999)}-{random.Next(10000, 99999)}",
                    Title = Title,
                    Course = Course,
                    FileUrl = File,
                    DateTime = DateTime.Now,
                    UserID = userID,
                    Status = "Active"
                });
                await _webDB.SaveChangesAsync();
                TempData["PopupScript"] = "<script>alert('Your book was created!');</script>";
            }
            else
            {
                TempData["PopupScript"] = "<script>alert('Insert Title, Course and Link appropriately!');</script>";
            }
            return RedirectToAction("AddBooks");
        }

        /// <summary>
        /// Adds a comment to a post.
        /// </summary>
        /// <param name="PostID">The ID of the post to comment on.</param>
        /// <param name="Content">The content of the comment.</param>
        /// <returns>Redirects to the Home index action.</returns>
        [HttpPost]
        public async Task<IActionResult> AddComment(string PostID, string Content)
        {
            string userID = HttpContext.Request.Cookies["UserID"] ?? HttpContext.Request.Cookies["AdminID"];
            _webDB.Comments.Add(new Comment
            {
                ID = $"{random.Next(10000, 99999)}-{random.Next(10000, 99999)}-{random.Next(10000, 99999)}",
                UserID = userID,
                PostID = PostID,
                Content = Content,
                Status = "Active"
            });
            await _webDB.SaveChangesAsync();
            TempData["PopupScript"] = "<script>alert('Comment Added');</script>";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Deletes a post by marking it as inactive.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>Redirects to the Home index action.</returns>
        public async Task<IActionResult> DeletePost(string id)
        {
            var post = await _webDB.Posts.Where(c => c.ID == id).FirstOrDefaultAsync();
            if (post != null)
            {
                post.Status = "Inactive";
                await _webDB.SaveChangesAsync();
                TempData["PopupScript"] = "<script>alert('Post Deleted');</script>";
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Calculates the CGPA based on the provided credit hours and grades.
        /// </summary>
        /// <param name="creditHours">List of credit hours for each course.</param>
        /// <param name="grade">List of grades for each course.</param>
        /// <param name="name">List of course names (optional).</param>
        /// <returns>Redirects to the CalculateCGPA view with the calculated CGPA.</returns>
        [HttpPost]
        public IActionResult Calculate(List<int> creditHours, List<double> grade, List<string> name)
        {
            if (creditHours == null || grade == null || creditHours.Count != grade.Count)
            {
                TempData["PopupScript"] = "<script>alert('Invalid Information');</script>";
                return RedirectToAction("CalculateCGPA");
            }

            double totalQualityPoints = 0;
            int totalCreditHours = 0;
            for (int i = 0; i < creditHours.Count; i++)
            {
                totalQualityPoints += creditHours[i] * grade[i];
                totalCreditHours += creditHours[i];
            }

            double cgpa = Math.Round(totalQualityPoints / totalCreditHours, 2);
            ViewBag.CGPA = cgpa;
            TempData["PopupScript"] = $"<script>alert('Your CGPA is: {cgpa}');</script>";

            return RedirectToAction("CalculateCGPA");
        }

        /// <summary>
        /// Displays the CalculateCGPA view.
        /// </summary>
        /// <returns>The CalculateCGPA view.</returns>
        public async Task<IActionResult> CalculateCGPA()
        {
            return View();
        }

        /// <summary>
        /// Updates a post with new content.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="postTitle">The new title of the post.</param>
        /// <param name="postContent">The new content of the post.</param>
        /// <param name="postImage">The new image URL of the post.</param>
        /// <returns>Redirects to the EditPost view with the updated post ID.</returns>
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

        /// <summary>
        /// Displays the EditPost view for a specific post.
        /// </summary>
        /// <param name="id">The ID of the post to edit.</param>
        /// <returns>The EditPost view.</returns>
        public async Task<IActionResult> EditPost(string id = "10000-10000-10000")
        {
            var post = await _webDB.Posts.Where(c => c.ID == id).FirstOrDefaultAsync();
            if (post != null)
            {
                ViewBag.ContentTitle = post.Title;
                ViewBag.Content = post.Content;
                ViewBag.ImgUrl = post.ImgUrl;
                ViewBag.ID = post.ID;
                return View();
            }
            return RedirectToAction("MakePost");
        }

        /// <summary>
        /// Creates a new post for the user.
        /// </summary>
        /// <param name="postTitle">The title of the post.</param>
        /// <param name="postContent">The content of the post.</param>
        /// <param name="postImage">The image URL of the post.</param>
        /// <returns>Redirects to the MakePost view.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePost(string postTitle, string postContent, string postImage)
        {
            string userID = HttpContext.Request.Cookies["UserID"] ?? HttpContext.Request.Cookies["AdminID"];
            if (!string.IsNullOrEmpty(postTitle) || !string.IsNullOrEmpty(postContent))
            {
                _webDB.Posts.Add(new Post
                {
                    ID = $"{random.Next(10000, 99999)}-{random.Next(10000, 99999)}-{random.Next(10000, 99999)}",
                    Title = postTitle,
                    Content = postContent,
                    ImgUrl = postImage,
                    DateTime = DateTime.Now,
                    UserID = userID,
                    Status = "Active"
                });
                await _webDB.SaveChangesAsync();
                TempData["PopupScript"] = "<script>alert('Your post was created!');</script>";
            }
            else
            {
                TempData["PopupScript"] = "<script>alert('Insert Title and Content appropriately!');</script>";
            }
            return RedirectToAction("MakePost");
        }

        /// <summary>
        /// Displays the MakePost view.
        /// </summary>
        /// <returns>The MakePost view.</returns>
        public async Task<IActionResult> MakePost()
        {
            return View();
        }

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="ID">The ID of the user to update.</param>
        /// <param name="FullName">The new full name of the user.</param>
        /// <param name="PhoneNumber">The new phone number of the user.</param>
        /// <param name="Email">The new email of the user.</param>
        /// <returns>Redirects to the UserDashboard view.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUser(string ID, string FullName, string PhoneNumber, string Email)
        {
            var user = await _webDB.Users.Where(c => c.ID == ID).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Name = FullName;
                user.PhoneNumber = PhoneNumber;
                user.Email = Email;

                Response.Cookies.Append("Name", FullName);
                Response.Cookies.Append("Email", Email);
                Response.Cookies.Append("PhoneNumber", PhoneNumber);

                await _webDB.SaveChangesAsync();
            }
            return RedirectToAction("UserDashboard");
        }

        /// <summary>
        /// Logs in a user or admin.
        /// </summary>
        /// <param name="emailInput">The email of the user.</param>
        /// <param name="passwordInput">The password of the user.</param>
        /// <returns>Redirects to the Home index action if successful, otherwise returns to the Login view with an error.</returns>
        [HttpPost]
        public async Task<IActionResult> LoginUser(string emailInput, string passwordInput)
        {
            var user = await _webDB.Users.Where(c => c.Email == emailInput && c.Password == passwordInput).FirstOrDefaultAsync();
            if (user == null)
            {
                TempData["PopupScript"] = "<script>alert('Invalid Credentials');</script>";
                return RedirectToAction("Login", "Home");
            }

            var options = new CookieOptions { Expires = DateTime.Now.AddDays(10) };

            if (user.AccountType == "Admin")
            {
                Response.Cookies.Append("AdminID", user.ID, options);
                Response.Cookies.Append("UserID", string.Empty, options);
            }
            else
            {
                Response.Cookies.Append("AdminID", string.Empty, options);
                Response.Cookies.Append("UserID", user.ID, options);
            }

            Response.Cookies.Append("Email", user.Email, options);
            Response.Cookies.Append("PhoneNumber", user.PhoneNumber, options);
            Response.Cookies.Append("Name", user.Name, options);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Signs out the user or admin by deleting their session cookies.
        /// </summary>
        /// <returns>Redirects to the Home index action.</returns>
        public IActionResult SignOut()
        {
            Response.Cookies.Delete("UserID");
            Response.Cookies.Delete("AdminID");
            Response.Cookies.Delete("Name");
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("PhoneNumber");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="nameInput">The name of the user.</param>
        /// <param name="emailInput">The email of the user.</param>
        /// <param name="phone">The phone number of the user.</param>
        /// <param name="passwordInput">The password of the user.</param>
        /// <returns>Redirects to the Home index action if successful, otherwise returns to the SignUp view with an error.</returns>
        [HttpPost]
        public async Task<IActionResult> SignUpUser(string nameInput, string emailInput, string phone, string passwordInput)
        {
            var user = await _webDB.Users.Where(c => c.Email == emailInput).FirstOrDefaultAsync();
            if (user != null)
            {
                TempData["PopupScript"] = "<script>alert('Email is already in use. Please try a different email.');</script>";
                return RedirectToAction("SignUp", "Home");
            }

            if (passwordInput.Length < 8)
            {
                TempData["PopupScript"] = "<script>alert('Password must be at least 8 characters long');</script>";
                return RedirectToAction("SignUp", "Home");
            }

            if (!phone.StartsWith("01"))
            {
                TempData["PopupScript"] = "<script>alert('Invalid Phone Number');</script>";
                return RedirectToAction("SignUp", "Home");
            }

            _webDB.Users.Add(new User
            {
                ID = $"{random.Next(10000, 99999)}-{random.Next(10000, 99999)}-{random.Next(10000, 99999)}",
                Name = nameInput,
                Email = emailInput,
                Password = passwordInput,
                PhoneNumber = phone,
                Status = "Active",
                AccountType = "User"
            });

            await _webDB.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
