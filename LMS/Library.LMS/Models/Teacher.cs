using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Teacher : User
    {
        public int YearsOfExperience { get; set; }
        public List<Course> Courses { get; } = new List<Course>();

    }
}
