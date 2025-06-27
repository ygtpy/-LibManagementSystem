using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models.Concrete;
using LibraryManagementSystem.Models.Enums;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;
        private readonly LoanRepository _loanRepository;

        // Events
        public event Action<Book> BookBorrowed;
        public event Action<Loan> BookReturned;
        public event Action<Member> MemberRegistered;
        public event Action<Loan> LoanOverdue;

        public LibraryService()
        {
            _bookRepository = new BookRepository();
            _memberRepository = new MemberRepository();
            _loanRepository = new LoanRepository();
        }

        #region Book Operations
        public async Task<Book> AddBookAsync(Book book)
        {
            var result = await _bookRepository.AddAsync(book);
            Console.WriteLine($"✅ Book '{result.Title}' added successfully.");
            return result;
        }

        public async Task<List<Book>> GetAvailableBookAsync()         
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }

        public async Task<List<Book>> SearchBookAsync(string searchTerm)
        {
            return await _bookRepository.SearchAsync(searchTerm);
        }
        #endregion

        #region Member Operations
        public async Task<Member> RegisterMemberAsync(Member member)
        {
            var result = await _memberRepository.AddAsync(member);
            MemberRegistered?.Invoke(result);
            Console.WriteLine($"✅ Member registered: {result.FullName} ({result.MembershipNumber})");
            return result;
        }

        public async Task<Member> GetMemberByNumberAsync(string membershipNumber)
        {
            return await _memberRepository.GetByMembershipNumberAsync(membershipNumber);
        }
        #endregion

        #region Loan Operations
        public async Task<Loan> BorrowBookAsync(string membershipNumber, int bookId)
        {
            var member = await GetMemberByNumberAsync(membershipNumber);
            if (member == null)
                throw new InvalidOperationException("Member not found!");

            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found!");

            if (!book.IsAvailable)                                    
                throw new InvalidOperationException("Book is not available for borrowing!");

            // Check member's active loans limit
            var activeLoans = await _loanRepository.GetActiveLoansByMemberAsync(membershipNumber);
            var maxBookLimit = GetMaxBooksLimit(member.MemberType);

            if (activeLoans.Count >= maxBookLimit)
                throw new InvalidOperationException($"Member has reached maximum book limit ({maxBookLimit})!"); 
            // Create loan
            var loan = new Loan
            {
                Member = member,
                Book = book,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(GetLoanPeriod(member.MemberType))
            };

            // update book status
            book.Status = BookStatus.Borrowed;
            await _bookRepository.UpdateAsync(book);

            // Save loan
            var result = await _loanRepository.AddAsync(loan);

            BookBorrowed?.Invoke(book);
            Console.WriteLine($"✅ Book borrowed: {book.Title} by {member.FullName}");

            return result;
        }

        public async Task<Loan> ReturnBookAsync(int loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
                throw new InvalidOperationException("Loan not found!");

            if (loan.Status != LoanStatus.Active)
                throw new InvalidOperationException("Loan is not active!");

            // Calculate fine if overdue
            if (loan.IsOverdue)
            {
                loan.Fine = loan.CalculateFine();
                loan.Member.TotalFines += loan.Fine;
                await _memberRepository.UpdateAsync(loan.Member);
            }

            // update loan
            loan.ReturnDate = DateTime.Now;
            loan.Status = LoanStatus.Returned;
            await _loanRepository.UpdateAsync(loan);

            // update book status
            loan.Book.Status = BookStatus.Available;
            await _bookRepository.UpdateAsync(loan.Book);

            BookReturned?.Invoke(loan);
            Console.WriteLine($"✅ Book returned: {loan.Book.Title}" +
                         (loan.Fine > 0 ? $" (Fine: {loan.Fine:C})" : "")); 

            return loan;
        }

        public async Task<List<Loan>> GetOverdueLoanAsync()  
        {
            return await _loanRepository.GetOverdueLoanAsync();     
        }
        #endregion

        #region Helper Methods
        private int GetMaxBooksLimit(MemberType memberType)
        {
            return memberType switch
            {
                MemberType.Student => 3,
                MemberType.Teacher => 10,
                MemberType.Staff => 5,
                MemberType.External => 2,
                _ => 3
            };
        }

        private int GetLoanPeriod(MemberType memberType)
        {
            return memberType switch
            {
                MemberType.Student => 14,  // 2 weeks
                MemberType.Teacher => 30,  // 1 month
                MemberType.Staff => 21,    // 3 weeks
                MemberType.External => 7,  // 1 week
                _ => 14
            };
        }
        #endregion

        #region Reports
        public async Task<Dictionary<string, int>> GetBookStatisticsAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return new Dictionary<string, int>
            {
                ["Total Books"] = books.Count,
                ["Available"] = books.Count(b => b.Status == BookStatus.Available),
                ["Borrowed"] = books.Count(b => b.Status == BookStatus.Borrowed),
                ["Reserved"] = books.Count(b => b.Status == BookStatus.Reserved),
                ["Lost"] = books.Count(b => b.Status == BookStatus.Lost)
            };
        }
        #endregion
    }
}