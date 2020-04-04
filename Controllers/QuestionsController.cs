using System.Linq;
using ESchool.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Models
{
    public class QuestionsController : Controller
    {
        public ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id, Question question)
        {
            var quiz = _context.Quizzes.First(q => q.Id == id);
            question.Quiz = quiz;
            question.Id = _context.Questions.Max(q => q.Id) + 1;
            _context.Add<Question>(question);
            _context.SaveChanges();
            return Redirect("/Quiz/Details/" + id);
        }

    }
}