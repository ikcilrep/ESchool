using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ESchool.Models
{
    public class Quiz
    {
        public string Name { get; set; }
        public IdentityUser Owner { get; set; }

        public List<Question> Questions { get; set; }

        public List<Participant> Participants { get; set; }

    }
}