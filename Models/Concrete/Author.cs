using LibraryManagementSystem.Models.Concrete; // Add this using directive
using LibraryManagementSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Concrete
{
    public class Author : Person
    {
        public string Biography { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

        public override void DisplayInfo()
        {
            Console.WriteLine($"Author: {FullName} - Books: {Books.Count}");
        }
    }
}
