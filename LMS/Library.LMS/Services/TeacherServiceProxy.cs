using Library.LMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace CLI.LMS.Helpers.Services
{
    public class TeacherServiceProxy
    {
        private static TeacherServiceProxy? instance;
        private static object instanceLock = new object();

        public static TeacherServiceProxy Current { get; private set; }


    }
}
