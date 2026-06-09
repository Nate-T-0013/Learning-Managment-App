using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class File
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Path { get; set; }
        public string TypeName => GetType().Name;

        public File()
        {
            Path = string.Empty;
        }
    }
}
