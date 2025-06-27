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
        // Kitap İşlemleri
        Task<Book> AddBookAsync(Book book);
        Task<List<Book>> GetAvailableBookAsync();      
        Task<List<Book>> SearchBookAsync(string searchTerm);

        // Üye İşlemleri
        Task<Member> RegisterMemberAsync(Member member);
        Task<Member> GetMemberByNumberAsync(string membershipNumber);

        // Kredi İşlemleri
        Task<Loan> BorrowBookAsync(string membershipNumber, int bookId);  
        Task<Loan> ReturnBookAsync(int loanId);
        Task<List<Loan>> GetOverdueLoanAsync();
        // Raporlar
        Task<Dictionary<string, int>> GetBookStatisticsAsync();
    }
}