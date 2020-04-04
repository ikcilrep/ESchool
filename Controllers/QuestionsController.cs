using System.Linq;
using ESchool.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Authorize]
        public IActionResult Edit(int id)
        {
            return View(_context.Questions.First(q => q.Id == id));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, Question question)
        {
            var questionToEdit = _context.Questions.First(q => q.Id == id);
            question.Id = questionToEdit.Id;
            question.Quiz = questionToEdit.Quiz;
            _context.Entry(questionToEdit).CurrentValues.SetValues(question);
            _context.SaveChanges();
            var quizId = _context.Quizzes.Include(q => q.Questions).First(q => q.Questions.Any(qu => qu.Id == id)).Id;
            return Redirect("/Quiz/Details/" + quizId);
        }
    }
}