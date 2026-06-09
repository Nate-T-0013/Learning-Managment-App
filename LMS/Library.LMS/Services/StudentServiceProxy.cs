using Library.LMS.Models;
using Library.LMS.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CLI.LMS.Helpers.Services
{
    public class StudentServiceProxy
    {
        private static StudentServiceProxy? instance;
        private static object instanceLock = new object();

        public int ActiveStudent { get; set; }

        public Student? GetById(int id)
        {
            if(id  == 0)
            {
                return null;
            }

            return Students.FirstOrDefault(i => i.Id == id);
        }

        public static StudentServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StudentServiceProxy();
                    }
                }
                return instance;
            }
        }

        private List<Student> students;
        public List<Student> Students => students;

        private StudentServiceProxy()
        {
            students = new List<Student>()
            {
                new Student() {Id = 1, Name = "John Doe", Code = "123", Classification = Classification.Senior }
                , new Student() {Id = 2, Name = "Billy Bob", Code = "456", Classification = Classification.Freshman }
                , new Student() {Id = 3, Name = "Jilly Joe", Code = "789", Classification = Classification.Sophomore }
                , new Student() {Id = 4, Name = "Ron Willy", Code = "987", Classification = Classification.Junior }
            };
        }

        private int LastKey => Students.Any() ? Students.Select(s => s.Id).Max() : 0;

        public Student? AddOrEdit(Student? student)
        {
            if(student == null)
            {
                return student;
            }
            
            if(student.Id == 0)
            {
                student.Id = LastKey + 1;
                students.Add(student);
            }

            return student;
        }

        public Student? Delete(Student? student)
        {
            if(student == null)
            {
                return null;
            }
            
            foreach(var course in CourseServiceProxy.Current.Courses)
            {
                if(course.Roster.Contains(student))
                {
                    CourseServiceProxy.Current.DeleteStudent(student, course.Id);
                }
            }

            students.Remove(student);
            return student;
        }
    }
}
