using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ESchool.Data;
using ESchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Quiz quiz)
        {
            quiz.Owner = CurrentUser;
            _context.Add(quiz);
            _context.SaveChanges();
            return Redirect("Details/" + quiz.Id);
        }

        [Authorize]

        public IActionResult Details(int id)
        {
            var quiz = _context.Quizzes.Include(q => q.Questions).Include(q => q.Owner).First(q => q.Id == id);
            if (quiz.Owner == CurrentUser)
            {
                return View(quiz);
            }
            return NotFound();
        }


        public IActionResult Index()
        {
            return View(_context.Quizzes.Where(q => q.Owner == CurrentUser));
        }

        public IActionResult Remove(int id)
        {
            var quiz = _context.Quizzes.First(q => q.Id == id);
            _context.Questions.RemoveRange(_context.Questions.Where(q => q.Quiz == quiz));
            _context.Quizzes.Remove(quiz);
            _context.SaveChanges();
            return View();
        }




    }
}