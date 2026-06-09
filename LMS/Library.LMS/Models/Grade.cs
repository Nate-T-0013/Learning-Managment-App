using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Grade
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public int TotalPercent { get; set; }
        public char LetterGrade => TotalPercent switch
        {
            >= 90 => 'A',
            >= 80 => 'B',
            >= 70 => 'C',
            >= 60 => 'D',
            _ => 'F'
        };

        public Grade()
        {

        }
    }
}
