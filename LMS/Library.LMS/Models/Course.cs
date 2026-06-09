using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Library.LMS.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Student> Roster { get; set; }
        public List<Module> Modules { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<AssignmentGroup> AssignmentGroups { get; set; }
        public List<Grade> Grades { get; set; }
        public Season Season { get; set; }
        public string SemesterSeason
        {
            get => Season.ToString();
            set
            {
                if (Enum.TryParse<Season>(value, true, out var result))
                {
                    Season = result;
                }
            }
        }
        public int SemesterYear { get; set; }
        public List<int> Sections { get; set; }
        public List<String> Announcements { get; set; }

        public Course()
        {
            Roster = new List<Student>();
            Modules = new List<Module>();
            Assignments = new List<Assignment>();
            AssignmentGroups = new List<AssignmentGroup>();
            Grades = new List<Grade>();
            Sections = new List<int>();
            Announcements = new List<String>();
        }
    }

    public enum Season
    {
        Winter = 'W', Spring = 'P', Summer = 'S', Fall = 'F'
    }
}
