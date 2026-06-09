using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Module
    {
        public int Id { get; set; }
        public List<Page> Pages { get; set; }
        public List<File> Files { get; set; }
        public List<Assignment> Assignments { get; set; }

        public IEnumerable<object> Items
        {
            get
            {
                return (Pages?.Cast<object>() ?? Enumerable.Empty<object>())
                    .Concat(Files?.Cast<object>() ?? Enumerable.Empty<object>())
                    .Concat(Assignments?.Cast<object>() ?? Enumerable.Empty<object>());
            }
        }

        public Module()
        {
            Pages = new List<Page>();
            Files = new List<File>();
            Assignments = new List<Assignment>();
        }
    }
}
