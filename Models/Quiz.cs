using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ESchool.Models
{
    public class Quiz
    {
        public string Name { get; set; }
        public IdentityUser Owner { get; set; }

        public List<Question> questions { get; set; }

        public List<Participant> participants { get; set; }

    }
}