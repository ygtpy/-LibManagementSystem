using LibraryManagementSystem.Models.Abstract;
using LibraryManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Concrete
{
    public class Book : BaseEntitiy
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public Author Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public string Category { get; set; }
        public BookStatus Status { get; set; } = BookStatus.Available;

        public void DisplayInfo()
        {
            Console.WriteLine($"Book: {Title} by {Author?.FullName} - Status: {Status}");
        }

        public bool IsAvailable => Status == BookStatus.Available;
    }
}
