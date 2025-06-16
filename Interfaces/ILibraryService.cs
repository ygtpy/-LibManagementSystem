using LibraryManagementSystem.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILibraryService
    {
        // Book Operations
        Task<Book> AddBookAsync(Book book);
        Task<List<Book>> GetAvailableBookAsync();
        Task<List<Book>> SearchBookAsync(string searchTerm);

        // Member Operations
        Task <Member> RegisterMemberAsync(Member member);
        Task<Member> GetMemberByNumberAsync(string membershipNumber);

        // Loan Operations
        Task<Loan> BorrowBookAsync(string memebershipNumber, int bookId);
        Task<Loan> ReturnBookAsync(int loanId);
        Task<List<Loan>> GetOverdueLoanAsync();

        // Reports
        Task<Dictionary<string, int>> GetBookStatisticsAsync();
    }
}
