using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ESchool.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Submitted { get; set; }

        public Quiz Quiz { get; set; }

        public int Points { get; set; }

        public List<Question> AnsweredQuestions { get; set; }
    }
}