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
            _context.Question.Add(question);
            _context.SaveChanges(); 
            return Redirect("/Quiz/Details/" + id);
        }

    }
}