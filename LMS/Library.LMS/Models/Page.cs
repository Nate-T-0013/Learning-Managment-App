using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string TypeName => GetType().Name;
    }
}
