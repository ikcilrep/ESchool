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
            var quiz = _context.Quizzes.Include(q => q.Participants).ThenInclude(p => p.User).Include(q => q.Questions).Include(q => q.Owner).First(q => q.Id == id);
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
            if (CurrentUser == quiz.Owner)
            {
                _context.Questions.RemoveRange(_context.Questions.Where(q => q.Quiz == quiz));
                _context.Participants.RemoveRange(_context.Participants.Where(q => q.Quiz == quiz));
                _context.Quizzes.Remove(quiz);
                _context.SaveChanges();

                return View();
            }
            return NotFound();
        }

        [Authorize]
        public IActionResult Play(int id)
        {
            var question = _context.Questions.First(q => q.Id == id);
            var quiz = _context.Quizzes.Include(q => q.Questions).Include(q => q.Participants).First(q => q.Questions.Contains(question));
            if (quiz.Finish <= DateTime.Now)
            {
                return View("QuizHasFinished");
            }

            if (quiz.Start > DateTime.Now)
            {
                return View("QuizHasntStarted", quiz.Start);
            }

            var participant = CurrentParticipant(quiz);
            if (participant.AnsweredQuestions.Contains(question))
            {
                return Redirect("/Quiz/Score/" + participant.Id);
            }
            return View(new Question
            {
                QuestionContent = question.QuestionContent,
                Answer1 = question.Answer1,
                Answer2 = question.Answer2,
                Answer3 = question.Answer3,
                Answer4 = question.Answer4
            });
        }

        private Participant CurrentParticipant(Quiz quiz)
        {
            if (!quiz.Participants.Any(p => p.User == CurrentUser))
            {
                var participant = new Participant { User = CurrentUser, Joined = DateTime.Now, Quiz = quiz };
                _context.Participants.Add(participant);
                _context.SaveChanges();
            }
            return _context.Participants.Include(p => p.AnsweredQuestions).Include(p => p.Quiz).First(p => p.Quiz == quiz && p.User == CurrentUser);
        }


        [Authorize]
        public IActionResult Score(int id)
        {
            var participant = _context.Participants.Include(q => q.User)
                                                   .Include(q => q.Quiz)
                                                   .Include(q => q.AnsweredQuestions)
                                                   .First(p => p.Id == id);
            if (participant.User == CurrentUser || participant.Quiz.Owner == CurrentUser)
            {
                return View(participant);
            }
            return NotFound();
        }



        [Authorize]
        [HttpPost]
        public IActionResult Play(int id, Question answeredQuestion)
        {
            var question = _context.Questions.First(q => q.Id == id);
            var quiz = _context.Quizzes.Include(q => q.Questions)
                                       .Include(q => q.Participants)
                                       .ThenInclude(p => p.User)
                                       .First(q => q.Questions.Contains(question));
            if (quiz.Finish <= DateTime.Now)
            {
                return View("QuizHasFinished");
            }

            if (quiz.Start > DateTime.Now)
            {
                return View("QuizHasntStarted", quiz.Start);
            }
            var participant = CurrentParticipant(quiz);

            var participantToUpdate = _context.Participants.First(p => p.Id == participant.Id);
            if (!participant.AnsweredQuestions.Contains(question))
            {
                participant.AnsweredQuestions.Add(question);
                _context.Entry(participantToUpdate).CurrentValues.SetValues(participant);
                _context.SaveChanges();

                if (question.IsAnswer1Correct == answeredQuestion.IsAnswer1Correct
                    && question.IsAnswer2Correct == answeredQuestion.IsAnswer2Correct
                    && question.IsAnswer3Correct == answeredQuestion.IsAnswer3Correct
                    && question.IsAnswer4Correct == answeredQuestion.IsAnswer4Correct)
                {
                    participant.Points++;
                    _context.Entry(participantToUpdate).CurrentValues.SetValues(participant);
                    _context.SaveChanges();
                    participantToUpdate = participant;
                }
            }

            if (quiz.Questions.Any(q => !participant.AnsweredQuestions.Contains(q)))
            {
                var nextQuestion = quiz.Questions.First(q => !participant.AnsweredQuestions.Contains(q));
                return Redirect("/Quiz/Play/" + nextQuestion.Id);
            }
            participant.Submitted = DateTime.Now;
            _context.Entry(participantToUpdate).CurrentValues.SetValues(participant);
            _context.SaveChanges();


            return Redirect("/Quiz/Score/" + participant.Id);
        }

        [Authorize]
        public IActionResult Scores()
        {
            var quizzes = _context.Quizzes.Include(q => q.Participants)
                                          .Where(q => q.Participants.Any(p => p.User == CurrentUser))
                                          .Select(q => new Tuple<Participant, Quiz>(_context.Participants.Include(p => p.Quiz)
                                                                                                         .First(p => p.Quiz == q && p.User == CurrentUser), q));
            return View(quizzes);
        }


    }
}