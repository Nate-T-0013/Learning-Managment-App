using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class AssignmentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Weight { get; set; }
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
