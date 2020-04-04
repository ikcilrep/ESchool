using ESchool.Data;
using ESchool.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Controllers
{
    public class QuizController : Controller
    {
        public ApplicationDbContext _context { get; set; }
        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Index(Question question)
        {
            _context.Add(question);
            return View();
        }
    }
}