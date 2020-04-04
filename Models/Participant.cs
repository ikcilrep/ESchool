using System;
using Microsoft.AspNetCore.Identity;

namespace ESchool.Models
{
    public class Participant
    {
        public IdentityUser User { get; set; }
        public DateTime Joined { get; set; }
        public DateTime Submitted { get; set; }
    }
}