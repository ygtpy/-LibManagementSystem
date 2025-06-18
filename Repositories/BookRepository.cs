using LibraryManagementSystem.Models.Concrete;
using LibraryManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository : BaseRepository<Book>
    {
        public BookRepository() : base("books.json") { }

        public override async Task<List<Book>> SearchAsync(string searchTerm)
        {
            return _entities.Where(b => b.IsActive &&
                (b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 b.Author?.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                 b.ISBN.Contains(searchTerm) ||
                 b.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            return _entities.Where(b => b.IsActive && b.Status == BookStatus.Available).ToList();
        }
    }
}
