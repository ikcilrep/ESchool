using System.Security.Claims;
using ESchool.Data;
using ESchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Controllers
{
    public class QuizController : Controller
    {
        public ApplicationDbContext _context { get; set; }
        private IdentityUser CurrentUser => _context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(Quiz quiz)
        {
            quiz.Owner = CurrentUser;
            _context.Add(quiz);
            _context.SaveChanges();
            return View();
        }
    }
}