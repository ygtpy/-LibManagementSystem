using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models.Concrete;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public class LibraryServices : ILibraryService
    {
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;
        private readonly LoanRepository _loanRepository;

        // Events
        public event Action<Book> BookBorrowed;
        public event Action<Loan> BookReturned;
        public event Action<Member> MemberRegistered;
        public event Action<Loan> LoanOverdue;

        public LibraryServices() 
        {
            _bookRepository = new BookRepository();
            _memberRepository = new MemberRepository();
            _loanRepository = new LoanRepository();
        }

        #region Book Operations
        public async Task<Book> AddBookAsync(Book book)
        {
            var result = await _bookRepository.AddAsync(book);
            Console.WriteLine($"Book '{result.Title}' added successfully.");
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
            Console.WriteLine($"Member registered: {result.FullName} ({result.MembershipNumber})");
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
            if(member == null)
                throw new InvalidOperationException("Member not found ");

            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found");

            if (!book.IsAvalible)
                throw new InvalidOperationException("Book is not available for borrowing");


            // Check member's active loans 
        }  


    }
}
