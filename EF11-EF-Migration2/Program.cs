using System;
using EF11.EntityFrameworkCoreMigration2.Data;
using Microsoft.EntityFrameworkCore;

namespace EF11_EF_Migration2
{
    public class Program
    {
        public static void Main()
        {
            using var context = new AppDbContext();
            foreach (var item in context.Sections.Include(c => c.Course))
            {
                Console.WriteLine($"Section : {item.SectionName} , Course : {item.Course.CourseName}");
            }
        }
    }
}