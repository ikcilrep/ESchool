using System.Linq;
using System.Security.Claims;
using ESchool.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Models
{
    public class QuestionsController : Controller
    {

        private IdentityUser CurrentUser => _context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));

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

        private int NewQuestionId
        {
            get
            {
                if (_context.Questions.Any())
                {
                    return _context.Questions.Max(q => q.Id) + 1;
                }
                return 0;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id, Question question)
        {
            var quiz = _context.Quizzes.First(q => q.Id == id);
            if (CurrentUser == quiz.Owner)
            {
                question.Quiz = quiz;
                question.Id = NewQuestionId;
                _context.Add<Question>(question);
                _context.SaveChanges();
                return Redirect("/Quiz/Details/" + id);
            }
            return NotFound();
        }

        [Authorize]
        public IActionResult Edit(int id)
        {

            var quiz = GetQuizFromQuestionId(id);
            if (CurrentUser == quiz.Owner)
            {
                return View(_context.Questions.First(q => q.Id == id));
            }
            return NotFound();
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
            var quizId = GetQuizFromQuestionId(id).Id;
            return Redirect("/Quiz/Details/" + quizId);
        }

        private Quiz GetQuizFromQuestionId(int id)
        {
            return _context.Quizzes.Include(q => q.Questions)
                                   .Include(q => q.Owner)
                                   .First(q => q.Questions.Any(qu => qu.Id == id));
        }
    }
}