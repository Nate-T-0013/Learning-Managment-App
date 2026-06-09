using System;
using System.Collections.Generic;
using System.Text;

namespace Library.LMS.Models
{
    public class Student : User
    {
        public Classification Classification { get; set; }


        public override string ToString()
        {
            return $"{Id}. {Name} - {Code} - {Classification}";
        }

        public string Display => ToString() ?? string.Empty;

        public string ClassificationString
        {
            get => Classification.ToString();
            set
            {
                if (Enum.TryParse<Classification>(value, true, out var result))
                {
                    Classification = result;
                }
            }
        }
    }

    public enum Classification { 
        Unknown = 'U', Freshman = 'F', Sophomore = 'S', Junior = 'J', Senior = 'N'
    }

}
