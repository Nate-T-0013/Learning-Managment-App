using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }
        public string Content { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int Points { get; set; }
        public string Comment { get; set; }
        
        public Submission()
        {
            Content = string.Empty;
            Comment = string.Empty;
        }
    }
}
