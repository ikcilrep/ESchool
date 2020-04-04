using System.Collections.Generic;

namespace ESchool.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }

        public string Answer1 { get; set; }

        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Answer4 { get; set; }

        public bool IsAnswer1Correct {get;set;}
        public bool IsAnswer2Correct {get;set;}
        public bool IsAnswer3Correct {get;set;}
        public bool IsAnswer4Correct {get;set;}

        public Quiz Quiz { get; set; }
    }
}