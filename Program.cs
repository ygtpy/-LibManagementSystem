using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models.Concrete;
using LibraryManagementSystem.Models.Enums;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using System.Linq;


class Program
{
    private static LibraryService _libraryService;

    static async Task Main(string[] args)
    {
        _libraryService = new LibraryService();

        // Subscribe to events
        _libraryService.BookBorrowed += OnBookBorrowed;
        _libraryService.BookReturned += OnBookReturned;
        _libraryService.MemberRegistered += OnMemberRegistered;

        // Add some sample data
        await AddSampleDataAsync();

        await ShowMainMenuAsync();
    }

    static async Task ShowMainMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("LIBRARY MANAGEMENT SYSTEM");

            Console.WriteLine("1. Book Management");
            Console.WriteLine("2. Member Management");
            Console.WriteLine("3. Loan Operations");
            Console.WriteLine("4. Reports");
            Console.WriteLine("5. Exit");
            Console.WriteLine();

            var choice = ConsoleHelper.ReadInput("Select an option (1-5)");

            switch (choice)
            {
                case "1":
                    await ShowBookMenuAsync();
                    break;
                case "2":
                    await ShowMemberMenuAsync();
                    break;
                case "3":
                    await ShowLoanMenuAsync();
                    break;
                case "4":
                    await ShowReportsMenuAsync();
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid option!");
                    ConsoleHelper.PressAnyKey();
                    break;
            }
        }
    }

    #region Book Management
    static async Task ShowBookMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("BOOK MANAGEMENT");

            Console.WriteLine("1. Add New Book");
            Console.WriteLine("2. Search Books");
            Console.WriteLine("3. List Available Books");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine();

            var choice = ConsoleHelper.ReadInput("Select an option (1-4)");

            switch (choice)
            {
                case "1":
                    await AddBookAsync();
                    break;
                case "2":
                    await SearchBooksAsync();
                    break;
                case "3":
                    await ListAvailableBooksAsync();
                    break;
                case "4":
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid option!");
                    ConsoleHelper.PressAnyKey();
                    break;
            }
        }
    }

    static async Task AddBookAsync()
    {
        ConsoleHelper.WriteHeader("ADD NEW BOOK");

        try
        {
            var title = ConsoleHelper.ReadInput("Book Title");
            var isbn = ConsoleHelper.ReadInput("ISBN");
            var authorFirstName = ConsoleHelper.ReadInput("Author First Name");
            var authorLastName = ConsoleHelper.ReadInput("Author Last Name");
            var publisher = ConsoleHelper.ReadInput("Publisher");
            var category = ConsoleHelper.ReadInput("Category");

            if (!DateTime.TryParse(ConsoleHelper.ReadInput("Publication Date (yyyy-mm-dd)"), out DateTime pubDate))
                pubDate = DateTime.Now;

            if (!int.TryParse(ConsoleHelper.ReadInput("Page Count"), out int pageCount))
                pageCount = 0;

            var book = new Book
            {
                Title = title,
                ISBN = isbn,
                Author = new Author
                {
                    FirstName = authorFirstName,
                    LastName = authorLastName
                },
                Publisher = publisher,
                Category = category,
                PublicationDate = pubDate,
                PageCount = pageCount
            };

            await _libraryService.AddBookAsync(book);
            ConsoleHelper.WriteSuccess("Book added successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error adding book: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task SearchBooksAsync()
    {
        ConsoleHelper.WriteHeader("SEARCH BOOKS");

        var searchTerm = ConsoleHelper.ReadInput("Enter search term (title, author, ISBN, category)");

        try
        {
            var books = await _libraryService.SearchBookAsync(searchTerm);

            if (books.Any())
            {
                Console.WriteLine($"\nFound {books.Count} book(s):");
                Console.WriteLine(new string('-', 80));

                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id} | Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author?.FullName} | Status: {book.Status}");
                    Console.WriteLine($"ISBN: {book.ISBN} | Category: {book.Category}");
                    Console.WriteLine(new string('-', 80));
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No books found!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error searching books: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ListAvailableBooksAsync()
    {
        ConsoleHelper.WriteHeader("AVAILABLE BOOKS");

        try
        {
            var books = await _libraryService.GetAvailableBookAsync();

            if (books.Any())
            {
                Console.WriteLine($"Available Books ({books.Count}):");
                Console.WriteLine(new string('-', 80));

                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id} | {book.Title} by {book.Author?.FullName}");
                    Console.WriteLine($"Category: {book.Category} | ISBN: {book.ISBN}");
                    Console.WriteLine(new string('-', 80));
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No available books!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error listing books: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
    #endregion

    #region Member Management
    static async Task ShowMemberMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("MEMBER MANAGEMENT");

            Console.WriteLine("1. Register New Member");
            Console.WriteLine("2. Search Members");
            Console.WriteLine("3. View Member Details");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine();

            var choice = ConsoleHelper.ReadInput("Select an option (1-4)");

            switch (choice)
            {
                case "1":
                    await RegisterMemberAsync();
                    break;
                case "2":
                    await SearchMembersAsync();
                    break;
                case "3":
                    await ViewMemberDetailsAsync();
                    break;
                case "4":
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid option!");
                    ConsoleHelper.PressAnyKey();
                    break;
            }
        }
    }

    static async Task RegisterMemberAsync()
    {
        ConsoleHelper.WriteHeader("REGISTER NEW MEMBER");

        try
        {
            var firstName = ConsoleHelper.ReadInput("First Name");
            var lastName = ConsoleHelper.ReadInput("Last Name");
            var email = ConsoleHelper.ReadInput("Email");
            var phone = ConsoleHelper.ReadInput("Phone");

            Console.WriteLine("\nMember Types:");
            Console.WriteLine("1. Student");
            Console.WriteLine("2. Teacher");
            Console.WriteLine("3. Staff");
            Console.WriteLine("4. External");

            var typeChoice = ConsoleHelper.ReadInput("Select member type (1-4)");

            var memberType = typeChoice switch
            {
                "1" => MemberType.Student,
                "2" => MemberType.Teacher,
                "3" => MemberType.Staff,
                "4" => MemberType.External,
                _ => MemberType.Student
            };

            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                MemberType = memberType
            };

            await _libraryService.RegisterMemberAsync(member);
            ConsoleHelper.WriteSuccess($"Member registered successfully! Membership Number: {member.MembershipNumber}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error registering member: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task SearchMembersAsync()
    {
        ConsoleHelper.WriteHeader("SEARCH MEMBERS");

        var searchTerm = ConsoleHelper.ReadInput("Enter search term (name, email, membership number)");

        try
        {
            var memberRepo = new MemberRepository();
            var members = await memberRepo.SearchAsync(searchTerm);

            if (members.Any())
            {
                Console.WriteLine($"\nFound {members.Count} member(s):");
                Console.WriteLine(new string('-', 80));

                foreach (var member in members)
                {
                    Console.WriteLine($"ID: {member.Id} | Name: {member.FullName}");
                    Console.WriteLine($"Membership #: {member.MembershipNumber} | Type: {member.MemberType}");
                    Console.WriteLine($"Email: {member.Email} | Phone: {member.Phone}");
                    Console.WriteLine($"Total Fines: {member.TotalFines:C}");
                    Console.WriteLine(new string('-', 80));
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No members found!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error searching members: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ViewMemberDetailsAsync()
    {
        ConsoleHelper.WriteHeader("MEMBER DETAILS");

        var membershipNumber = ConsoleHelper.ReadInput("Enter membership number");

        try
        {
            var member = await _libraryService.GetMemberByNumberAsync(membershipNumber);

            if (member != null)
            {
                Console.WriteLine($"\n📋 Member Information:");
                Console.WriteLine($"Name: {member.FullName}");
                Console.WriteLine($"Membership Number: {member.MembershipNumber}");
                Console.WriteLine($"Type: {member.MemberType}");
                Console.WriteLine($"Email: {member.Email}");
                Console.WriteLine($"Phone: {member.Phone}");
                Console.WriteLine($"Member Since: {member.MembershipDate:dd/MM/yyyy}");
                Console.WriteLine($"Total Fines: {member.TotalFines:C}");

                // Show active loans
                var loanRepo = new LoanRepository();
                var activeLoans = await loanRepo.GetActiveLoansByMemberAsync(membershipNumber);

                if (activeLoans.Any())
                {
                    Console.WriteLine($"\n📚 Active Loans ({activeLoans.Count}):");
                    foreach (var loan in activeLoans)
                    {
                        Console.WriteLine($"- {loan.Book.Title} (Due: {loan.DueDate:dd/MM/yyyy})" +
                                        (loan.IsOverdue ? " ⚠️ OVERDUE" : ""));
                    }
                }
                else
                {
                    Console.WriteLine("\n📚 No active loans");
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("Member not found!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error viewing member details: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
    #endregion

    #region Loan Operations
    static async Task ShowLoanMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("LOAN OPERATIONS");

            Console.WriteLine("1. Borrow Book");
            Console.WriteLine("2. Return Book");
            Console.WriteLine("3. View Overdue Loans");
            Console.WriteLine("4. View All Active Loans");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine();

            var choice = ConsoleHelper.ReadInput("Select an option (1-5)");

            switch (choice)
            {
                case "1":
                    await BorrowBookAsync();
                    break;
                case "2":
                    await ReturnBookAsync();
                    break;
                case "3":
                    await ViewOverdueLoansAsync();
                    break;
                case "4":
                    await ViewActiveLoansAsync();
                    break;
                case "5":
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid option!");
                    ConsoleHelper.PressAnyKey();
                    break;
            }
        }
    }

    static async Task BorrowBookAsync()
    {
        ConsoleHelper.WriteHeader("BORROW BOOK");

        try
        {
            var membershipNumber = ConsoleHelper.ReadInput("Member's Membership Number");

            // Verify member exists
            var member = await _libraryService.GetMemberByNumberAsync(membershipNumber);
            if (member == null)
            {
                ConsoleHelper.WriteError("Member not found!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            Console.WriteLine($"\nMember: {member.FullName} ({member.MemberType})");

            // Show available books
            var availableBooks = await _libraryService.GetAvailableBookAsync();
            if (!availableBooks.Any())
            {
                ConsoleHelper.WriteWarning("No books available for borrowing!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            Console.WriteLine("\n📚 Available Books:");
            Console.WriteLine(new string('-', 60));

            foreach (var book in availableBooks.Take(10)) // ilk 10 göster
            {
                Console.WriteLine($"ID: {book.Id} | {book.Title} by {book.Author?.FullName}");
            }

            if (availableBooks.Count > 10)
            {
                Console.WriteLine($"... and {availableBooks.Count - 10} more books");
                Console.WriteLine("Use search functionality to find specific books");
            }

            Console.WriteLine(new string('-', 60));

            if (!int.TryParse(ConsoleHelper.ReadInput("Enter Book ID to borrow"), out int bookId))
            {
                ConsoleHelper.WriteError("Invalid Book ID!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            var loan = await _libraryService.BorrowBookAsync(membershipNumber, bookId);

            ConsoleHelper.WriteSuccess("Book borrowed successfully!");
            Console.WriteLine($"Due Date: {loan.DueDate:dd/MM/yyyy}");
            Console.WriteLine($"Loan ID: {loan.Id} (keep this for return)");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error borrowing book: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ReturnBookAsync()
    {
        ConsoleHelper.WriteHeader("RETURN BOOK");

        try
        {
            
            Console.WriteLine("Return Methods:");
            Console.WriteLine("1. Use Loan ID");
            Console.WriteLine("2. Search by Member");

            var method = ConsoleHelper.ReadInput("Select method (1-2)");

            if (method == "1")
            {
                if (!int.TryParse(ConsoleHelper.ReadInput("Enter Loan ID"), out int loanId))
                {
                    ConsoleHelper.WriteError("Invalid Loan ID!");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var loan = await _libraryService.ReturnBookAsync(loanId);
                ConsoleHelper.WriteSuccess("Book returned successfully!");

                if (loan.Fine > 0)
                {
                    ConsoleHelper.WriteWarning($"Late return fine: {loan.Fine:C}");
                }
            }
            else if (method == "2")
            {
                var membershipNumber = ConsoleHelper.ReadInput("Enter Membership Number");

                var loanRepo = new LoanRepository();
                var activeLoans = await loanRepo.GetActiveLoansByMemberAsync(membershipNumber);

                if (!activeLoans.Any())
                {
                    ConsoleHelper.WriteWarning("No active loans found for this member!");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine("\n📚 Active Loans:");
                foreach (var activeLoan in activeLoans)
                {
                    Console.WriteLine($"Loan ID: {activeLoan.Id} | Book: {activeLoan.Book.Title}");
                    Console.WriteLine($"Due: {activeLoan.DueDate:dd/MM/yyyy}" +
                                    (activeLoan.IsOverdue ? " ⚠️ OVERDUE" : ""));
                    Console.WriteLine(new string('-', 50));
                }

                if (!int.TryParse(ConsoleHelper.ReadInput("Enter Loan ID to return"), out int selectedLoanId))
                {
                    ConsoleHelper.WriteError("Invalid Loan ID!");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var returnedLoan = await _libraryService.ReturnBookAsync(selectedLoanId);
                ConsoleHelper.WriteSuccess("Book returned successfully!");

                if (returnedLoan.Fine > 0)
                {
                    ConsoleHelper.WriteWarning($"Late return fine: {returnedLoan.Fine:C}");
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error returning book: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ViewOverdueLoansAsync()
    {
        ConsoleHelper.WriteHeader("OVERDUE LOANS");

        try
        {
            var overdueLoans = await _libraryService.GetOverdueLoanAsync();

            if (overdueLoans.Any())
            {
                Console.WriteLine($"⚠️ Found {overdueLoans.Count} overdue loan(s):\n");

                foreach (var loan in overdueLoans)
                {
                    var overdueDays = (DateTime.Now - loan.DueDate).Days;
                    var fine = loan.CalculateFine();

                    Console.WriteLine($"Loan ID: {loan.Id}");
                    Console.WriteLine($"Member: {loan.Member.FullName} ({loan.Member.MembershipNumber})");
                    Console.WriteLine($"Book: {loan.Book.Title}");
                    Console.WriteLine($"Due Date: {loan.DueDate:dd/MM/yyyy}");
                    Console.WriteLine($"Days Overdue: {overdueDays}");
                    Console.WriteLine($"Current Fine: {fine:C}");
                    Console.WriteLine(new string('-', 60));
                }

                var totalFines = overdueLoans.Sum(l => l.CalculateFine());
                Console.WriteLine($"\nTotal Outstanding Fines: {totalFines:C}");
            }
            else
            {
                ConsoleHelper.WriteSuccess("No overdue loans! 🎉");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error viewing overdue loans: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ViewActiveLoansAsync()
    {
        ConsoleHelper.WriteHeader("ALL ACTIVE LOANS");

        try
        {
            var loanRepo = new LoanRepository();
            var allLoans = await loanRepo.GetAllAsync();
            var activeLoans = allLoans.Where(l => l.Status == LoanStatus.Active).ToList();

            if (activeLoans.Any())
            {
                Console.WriteLine($"📚 Found {activeLoans.Count} active loan(s):\n");

                foreach (var loan in activeLoans)
                {
                    Console.WriteLine($"Loan ID: {loan.Id}");
                    Console.WriteLine($"Member: {loan.Member.FullName}");
                    Console.WriteLine($"Book: {loan.Book.Title}");
                    Console.WriteLine($"Loan Date: {loan.LoanDate:dd/MM/yyyy}");
                    Console.WriteLine($"Due Date: {loan.DueDate:dd/MM/yyyy}");

                    if (loan.IsOverdue)
                    {
                        ConsoleHelper.WriteWarning($"⚠️ OVERDUE - Fine: {loan.CalculateFine():C}");
                    }
                    else
                    {
                        var daysLeft = (loan.DueDate - DateTime.Now).Days;
                        Console.WriteLine($"Days Remaining: {daysLeft}");
                    }

                    Console.WriteLine(new string('-', 60));
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No active loans found!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error viewing active loans: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
    #endregion

    #region Reports
    static async Task ShowReportsMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("REPORTS & STATISTICS");

            Console.WriteLine("1. Book Statistics");
            Console.WriteLine("2. Member Statistics");
            Console.WriteLine("3. Loan Statistics");
            Console.WriteLine("4. Popular Books Report");
            Console.WriteLine("5. Member Activity Report");
            Console.WriteLine("6. Financial Report");
            Console.WriteLine("7. Back to Main Menu");
            Console.WriteLine();

            var choice = ConsoleHelper.ReadInput("Select an option (1-7)");

            switch (choice)
            {
                case "1":
                    await ShowBookStatisticsAsync();
                    break;
                case "2":
                    await ShowMemberStatisticsAsync();
                    break;
                case "3":
                    await ShowLoanStatisticsAsync();
                    break;
                case "4":
                    await ShowPopularBooksReportAsync();
                    break;
                case "5":
                    await ShowMemberActivityReportAsync();
                    break;
                case "6":
                    await ShowFinancialReportAsync();
                    break;
                case "7":
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid option!");
                    ConsoleHelper.PressAnyKey();
                    break;
            }
        }
    }

    static async Task ShowBookStatisticsAsync()
    {
        ConsoleHelper.WriteHeader("BOOK STATISTICS");

        try
        {
            var stats = await _libraryService.GetBookStatisticsAsync();

            Console.WriteLine("📚 Book Collection Overview:\n");

            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value}");
            }

            // ek istatistikler
            var bookRepo = new BookRepository();
            var allBooks = await bookRepo.GetAllAsync();

            Console.WriteLine($"\n📊 Additional Statistics:");
            Console.WriteLine($"Average Page Count: {allBooks.Average(b => b.PageCount):F0} pages");

            var categoryCounts = allBooks.GroupBy(b => b.Category)
                                       .OrderByDescending(g => g.Count())
                                       .Take(5);

            Console.WriteLine($"\n🏷️ Top Categories:");
            foreach (var category in categoryCounts)
            {
                Console.WriteLine($"- {category.Key}: {category.Count()} books");
            }

            var recentBooks = allBooks.Where(b => b.CreatedDate >= DateTime.Now.AddDays(-30))
                                     .Count();
            Console.WriteLine($"\n📅 Books added in last 30 days: {recentBooks}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating book statistics: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ShowMemberStatisticsAsync()
    {
        ConsoleHelper.WriteHeader("MEMBER STATISTICS");

        try
        {
            var memberRepo = new MemberRepository();
            var allMembers = await memberRepo.GetAllAsync();

            Console.WriteLine("👥 Member Overview:\n");
            Console.WriteLine($"Total Members: {allMembers.Count}");

            var memberTypeStats = allMembers.GroupBy(m => m.MemberType)
                                           .OrderByDescending(g => g.Count());

            Console.WriteLine($"\n📊 Members by Type:");
            foreach (var type in memberTypeStats)
            {
                Console.WriteLine($"- {type.Key}: {type.Count()} members");
            }

            var newMembers = allMembers.Where(m => m.MembershipDate >= DateTime.Now.AddDays(-30))
                                      .Count();
            Console.WriteLine($"\n📅 New members in last 30 days: {newMembers}");

            var membersWithFines = allMembers.Where(m => m.TotalFines > 0).ToList();
            Console.WriteLine($"\n💰 Members with outstanding fines: {membersWithFines.Count}");

            if (membersWithFines.Any())
            {
                var totalFines = membersWithFines.Sum(m => m.TotalFines);
                Console.WriteLine($"Total outstanding fines: {totalFines:C}");
            }

            // aktif borçlular
            var loanRepo = new LoanRepository();
            var activeLoans = await loanRepo.GetAllAsync();
            var activeBorrowers = activeLoans.Where(l => l.Status == LoanStatus.Active)
                                            .Select(l => l.Member.MembershipNumber)
                                            .Distinct()
                                            .Count();

            Console.WriteLine($"\n📚 Members with active loans: {activeBorrowers}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating member statistics: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ShowLoanStatisticsAsync()
    {
        ConsoleHelper.WriteHeader("LOAN STATISTICS");

        try
        {
            var loanRepo = new LoanRepository();
            var allLoans = await loanRepo.GetAllAsync();

            Console.WriteLine("📊 Loan Overview:\n");
            Console.WriteLine($"Total Loans (All Time): {allLoans.Count}");

            var activeLoans = allLoans.Where(l => l.Status == LoanStatus.Active).ToList();
            var returnedLoans = allLoans.Where(l => l.Status == LoanStatus.Returned).ToList();
            var overdueLoans = allLoans.Where(l => l.IsOverdue).ToList();

            Console.WriteLine($"Active Loans: {activeLoans.Count}");
            Console.WriteLine($"Returned Loans: {returnedLoans.Count}");
            Console.WriteLine($"Overdue Loans: {overdueLoans.Count}");

            // Son aktivite
            var recentLoans = allLoans.Where(l => l.LoanDate >= DateTime.Now.AddDays(-30))
                                     .Count();
            Console.WriteLine($"\n📅 Loans in last 30 days: {recentLoans}");

            var recentReturns = allLoans.Where(l => l.ReturnDate.HasValue &&
                                                  l.ReturnDate >= DateTime.Now.AddDays(-30))
                                       .Count();
            Console.WriteLine($"Returns in last 30 days: {recentReturns}");

            // İade edilen kitabın ortalama ödünç alma süresi
            if (returnedLoans.Any())
            {
                var avgDuration = returnedLoans.Where(l => l.ReturnDate.HasValue)
                                              .Average(l => (l.ReturnDate.Value - l.LoanDate).TotalDays);
                Console.WriteLine($"\n⏱️ Average loan duration: {avgDuration:F1} days");
            }

            // Geç iade oranı
            if (returnedLoans.Any())
            {
                var lateReturns = returnedLoans.Where(l => l.ReturnDate > l.DueDate).Count();
                var lateReturnRate = (lateReturns * 100.0) / returnedLoans.Count;
                Console.WriteLine($"📈 Late return rate: {lateReturnRate:F1}%");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating loan statistics: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ShowPopularBooksReportAsync()
    {
        ConsoleHelper.WriteHeader("POPULAR BOOKS REPORT");

        try
        {
            var loanRepo = new LoanRepository();
            var allLoans = await loanRepo.GetAllAsync();

            var popularBooks = allLoans.GroupBy(l => new { l.Book.Id, l.Book.Title, l.Book.Author.FullName })
                                      .Select(g => new
                                      {
                                          BookId = g.Key.Id,
                                          Title = g.Key.Title,
                                          Author = g.Key.FullName,
                                          LoanCount = g.Count(),
                                          CurrentlyBorrowed = g.Any(l => l.Status == LoanStatus.Active)
                                      })
                                      .OrderByDescending(b => b.LoanCount)
                                      .Take(10)
                                      .ToList();

            if (popularBooks.Any())
            {
                Console.WriteLine("📈 Top 10 Most Borrowed Books:\n");

                for (int i = 0; i < popularBooks.Count; i++)
                {
                    var book = popularBooks[i];
                    Console.WriteLine($"{i + 1}. {book.Title} by {book.Author}");
                    Console.WriteLine($"   Times borrowed: {book.LoanCount}");
                    Console.WriteLine($"   Status: {(book.CurrentlyBorrowed ? "Currently Borrowed" : "Available")}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No loan history available for popularity analysis!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating popular books report: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ShowMemberActivityReportAsync()
    {
        ConsoleHelper.WriteHeader("MEMBER ACTIVITY REPORT");

        try
        {
            var loanRepo = new LoanRepository();
            var allLoans = await loanRepo.GetAllAsync();

            var memberActivity = allLoans.GroupBy(l => new
            {
                l.Member.MembershipNumber,
                l.Member.FullName,
                l.Member.MemberType
            })
                                        .Select(g => new
                                        {
                                            MembershipNumber = g.Key.MembershipNumber,
                                            FullName = g.Key.FullName,
                                            MemberType = g.Key.MemberType,
                                            TotalLoans = g.Count(),
                                            ActiveLoans = g.Count(l => l.Status == LoanStatus.Active),
                                            OverdueLoans = g.Count(l => l.IsOverdue),
                                            LastLoanDate = g.Max(l => l.LoanDate)
                                        })
                                        .OrderByDescending(m => m.TotalLoans)
                                        .Take(10)
                                        .ToList();

            if (memberActivity.Any())
            {
                Console.WriteLine("👑 Top 10 Most Active Members:\n");

                for (int i = 0; i < memberActivity.Count; i++)
                {
                    var member = memberActivity[i];
                    Console.WriteLine($"{i + 1}. {member.FullName} ({member.MemberType})");
                    Console.WriteLine($"   Membership #: {member.MembershipNumber}");
                    Console.WriteLine($"   Total loans: {member.TotalLoans}");
                    Console.WriteLine($"   Active loans: {member.ActiveLoans}");

                    if (member.OverdueLoans > 0)
                    {
                        ConsoleHelper.WriteWarning($"   ⚠️ Overdue loans: {member.OverdueLoans}");
                    }

                    Console.WriteLine($"   Last loan: {member.LastLoanDate:dd/MM/yyyy}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.WriteWarning("No member activity data available!");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating member activity report: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }

    static async Task ShowFinancialReportAsync()
    {
        ConsoleHelper.WriteHeader("FINANCIAL REPORT");

        try
        {
            var memberRepo = new MemberRepository();
            var loanRepo = new LoanRepository();

            var allMembers = await memberRepo.GetAllAsync();
            var allLoans = await loanRepo.GetAllAsync();

            Console.WriteLine("💰 Financial Overview:\n");

            // Ödenmemiş para cezaları
            var totalOutstandingFines = allMembers.Sum(m => m.TotalFines);
            Console.WriteLine($"Total Outstanding Fines: {totalOutstandingFines:C}");

            // Tahsil edilen cezalar (iade edilen kredilerden)
            var collectedFines = allLoans.Where(l => l.Status == LoanStatus.Returned && l.Fine > 0)
                                        .Sum(l => l.Fine);
            Console.WriteLine($"Total Collected Fines: {collectedFines:C}");

            // Mevcut ay etkinliği
            var currentMonthLoans = allLoans.Where(l => l.LoanDate.Month == DateTime.Now.Month &&
                                                       l.LoanDate.Year == DateTime.Now.Year)
                                           .Count();
            Console.WriteLine($"\n📅 Current Month Activity:");
            Console.WriteLine($"New loans this month: {currentMonthLoans}");

            var currentMonthFines = allLoans.Where(l => l.ReturnDate?.Month == DateTime.Now.Month &&
                                                       l.ReturnDate?.Year == DateTime.Now.Year &&
                                                       l.Fine > 0)
                                           .Sum(l => l.Fine);
            Console.WriteLine($"Fines collected this month: {currentMonthFines:C}");

            // En yüksek cezayı alan üyeler
            var membersWithFines = allMembers.Where(m => m.TotalFines > 0)
                                            .OrderByDescending(m => m.TotalFines)
                                            .Take(5)
                                            .ToList();

            if (membersWithFines.Any())
            {
                Console.WriteLine($"\n🔝 Top 5 Members with Outstanding Fines:");

                foreach (var member in membersWithFines)
                {
                    Console.WriteLine($"- {member.FullName}: {member.TotalFines:C}");
                }
            }

            Console.WriteLine($"\n📊 Fine Statistics:");
            Console.WriteLine($"Members with fines: {allMembers.Count(m => m.TotalFines > 0)}");
            Console.WriteLine($"Average fine per member: {(totalOutstandingFines / Math.Max(allMembers.Count, 1)):C}");

            if (totalOutstandingFines > 0)
            {
                var collectionRate = (collectedFines / (collectedFines + totalOutstandingFines)) * 100;
                Console.WriteLine($"Fine collection rate: {collectionRate:F1}%");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Error generating financial report: {ex.Message}");
        }

        ConsoleHelper.PressAnyKey();
    }
    #endregion

    #region Event Handlers
    static void OnBookBorrowed(Book book)
    {
        Console.WriteLine($"📚 Event: Book '{book.Title}' has been borrowed!");
    }

    static void OnBookReturned(Loan loan)
    {
        Console.WriteLine($"📚 Event: Book '{loan.Book.Title}' has been returned!");
    }

    static void OnMemberRegistered(Member member)
    {
        Console.WriteLine($"👤 Event: New member '{member.FullName}' registered!");
    }
    #endregion

    #region Sample Data
    static async Task AddSampleDataAsync()
    {
        // Örnek kitaplar ekle
        var books = new List<Book>
        {
            new Book { Title = "C# Programming", ISBN = "978-1234567890",
                      Author = new Author { FirstName = "John", LastName = "Doe" },
                      Category = "Programming", Publisher = "Tech Books" },
            new Book { Title = "Database Design", ISBN = "978-0987654321",
                      Author = new Author { FirstName = "Jane", LastName = "Smith" },
                      Category = "Database", Publisher = "Data Press" }
        };

        foreach (var book in books)
        {
            try { await _libraryService.AddBookAsync(book); }
            catch { /* Ignore if already exists */ }
        }

        // Örnek üyeler
        var members = new List<Member>
        {
            new Member { FirstName = "Alice", LastName = "Johnson",
                        Email = "alice@email.com", MemberType = MemberType.Student },
            new Member { FirstName = "Bob", LastName = "Wilson",
                        Email = "bob@email.com", MemberType = MemberType.Teacher }
        };

        foreach (var member in members)
        {
            try { await _libraryService.RegisterMemberAsync(member); }
            catch { /* Ignore if already exists */ }
        }
    }
    #endregion

    #region

    

    #endregion

   
}