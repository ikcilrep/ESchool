using System.Collections.Generic;

namespace ESchool.Models
{
    public class Question
    {
        public string QuestionContent { get; set; }

        public string Answer1 {get; set;}

        public string Answer2 {get; set;}

        public string Answer3 {get; set;}

        public string Answer4 {get; set;}

        public int CorrectAnswer { get; set; }
    }
}