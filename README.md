# 📚 Library Management System - Console Application

**A comprehensive console-based library management system built with C# demonstrating advanced programming concepts and best practices.**

[![.NET](https://img.shields.io/badge/.NET-6.0+-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-10.0+-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## 🎯 Project Overview

This Library Management System is a feature-rich console application designed to demonstrate advanced C# programming concepts including Object-Oriented Programming, SOLID principles, design patterns, and modern development practices. The system manages books, members, and loan operations with a focus on clean architecture and maintainable code.

## ✨ Key Features

### 📖 **Book Management**
- ✅ Add new books with author information
- ✅ Search books by title, author, ISBN, or category
- ✅ List all available books
- ✅ Automatic ISBN validation
- ✅ Category-based organization
- ✅ Status tracking (Available, Borrowed, Reserved, Lost)

### 👥 **Member Management**
- ✅ Register new members with different types (Student, Teacher, Staff, External)
- ✅ Automatic membership number generation
- ✅ Search members by name, email, or membership number
- ✅ View detailed member profiles with loan history
- ✅ Fine tracking and management
- ✅ Member type-based borrowing limits

### 🔄 **Loan Operations**
- ✅ Book borrowing with business rule validation
- ✅ Automated due date calculation based on member type
- ✅ Book return processing with fine calculation
- ✅ Overdue loan tracking and notifications
- ✅ Active loan management
- ✅ Member-specific loan limits

### 📊 **Comprehensive Reporting**
- ✅ Book statistics and collection overview
- ✅ Member statistics by type and activity
- ✅ Loan statistics and trends
- ✅ Popular books report
- ✅ Member activity analysis
- ✅ Financial reporting with fine tracking

### 🛠️ **Advanced Features**
- ✅ Event-driven architecture with real-time notifications
- ✅ JSON-based data persistence
- ✅ Colored console output for better user experience
- ✅ Input validation and error handling
- ✅ Clean, organized menu system

## 🏗️ Architecture & Design Patterns

### **Object-Oriented Programming Concepts:**
- **Inheritance** - `Person` base class for `Author` and `Member`
- **Polymorphism** - Virtual methods and interface implementations
- **Encapsulation** - Private fields with public properties
- **Abstraction** - Abstract base classes and interfaces

### **SOLID Principles Applied:**
- ✅ **Single Responsibility** - Each class has one clear purpose
- ✅ **Open/Closed** - Extensible without modification
- ✅ **Liskov Substitution** - Proper inheritance hierarchy
- ✅ **Interface Segregation** - Focused interfaces like `IRepository<T>`
- ✅ **Dependency Inversion** - Service layer depends on abstractions

### **Design Patterns Implemented:**
- **Repository Pattern** - Data access abstraction
- **Strategy Pattern** - Different loan policies for member types
- **Observer Pattern** - Event-driven notifications
- **Factory Pattern** - Entity creation and validation

## 🚀 Getting Started

### Prerequisites
- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
- Visual Studio 2022, VS Code, or any C# compatible IDE

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/ygtpy/library-management-console.git
   cd library-management-console
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

### First Run Setup
The application will automatically:
- Create necessary data directories (`Data/`, `Logs/`)
- Generate sample data for testing
- Initialize JSON storage files
- Display the main menu

## 📋 Usage Guide

### Main Menu Navigation
```
═══════════════════════════════════════
   LIBRARY MANAGEMENT SYSTEM
═══════════════════════════════════════

1. Book Management
2. Member Management
3. Loan Operations
4. Reports
5. Exit
```

### Book Management Operations
- **Add New Book**: Enter book details including title, ISBN, author, and category
- **Search Books**: Find books by any criteria (title, author, ISBN, category)
- **List Available Books**: View all books currently available for borrowing

### Member Management Operations
- **Register New Member**: Create member accounts with type-specific privileges
- **Search Members**: Find members by name, email, or membership number
- **View Member Details**: See complete member profile with active loans and history

### Loan Operations
- **Borrow Book**: Process book loans with automatic validation
- **Return Book**: Handle book returns with fine calculation
- **View Overdue Loans**: Monitor and track late returns
- **View All Active Loans**: Complete overview of current borrowings

### Reports & Analytics
- **Book Statistics**: Collection overview and status breakdown
- **Member Statistics**: Demographics and activity analysis
- **Loan Statistics**: Borrowing patterns and trends
- **Popular Books Report**: Most borrowed titles
- **Member Activity Report**: Top active users
- **Financial Report**: Fine collection and outstanding amounts

## 🗂️ Project Structure

```
LibraryManagementSystem/
├── 📁 Models/
│   ├── 📁 Abstract/          # Base classes (BaseEntity, Person)
│   ├── 📁 Concrete/          # Entity implementations (Book, Member, Loan, Author)
│   └── 📁 Enums/             # Enumeration types (BookStatus, MemberType, LoanStatus)
├── 📁 Interfaces/            # Service contracts (ILibraryService, IRepository<T>)
├── 📁 Repositories/          # Data access layer (BookRepository, MemberRepository, LoanRepository)
├── 📁 Services/              # Business logic layer (LibraryService)
├── 📁 Utilities/             # Helper classes (ConsoleHelper)
├── 📁 Data/                  # JSON data storage (books.json, members.json, loans.json)
├── 📁 Logs/                  # Application logs (auto-generated)
└── 📄 Program.cs             # Application entry point and UI logic
```

## 🧪 Key C# Concepts Demonstrated

### **Modern C# Features**
```csharp
// Pattern Matching
private int GetMaxBooksLimit(MemberType memberType) => memberType switch
{
    MemberType.Student => 3,
    MemberType.Teacher => 10,
    MemberType.Staff => 5,
    MemberType.External => 2,
    _ => 3
};

// Nullable Reference Types
public Author? Author { get; set; }

// Expression-bodied Members
public string FullName => $"{FirstName} {LastName}";
public bool IsAvailable => Status == BookStatus.Available;
```

### **Async Programming**
```csharp
public async Task<Book> AddBookAsync(Book book)
{
    var result = await _bookRepository.AddAsync(book);
    Console.WriteLine($"✅ Book '{result.Title}' added successfully.");
    return result;
}
```

### **LINQ Queries**
```csharp
var popularBooks = allLoans.GroupBy(l => new { l.Book.Id, l.Book.Title })
                          .Select(g => new { Title = g.Key.Title, Count = g.Count() })
                          .OrderByDescending(b => b.Count)
                          .Take(10);
```

### **Events & Delegates**
```csharp
public event Action<Book> BookBorrowed;
public event Action<Loan> BookReturned;
public event Action<Member> MemberRegistered;

// Event firing
BookBorrowed?.Invoke(book);
```

### **Exception Handling**
```csharp
try
{
    var loan = await _libraryService.BorrowBookAsync(membershipNumber, bookId);
    ConsoleHelper.WriteSuccess("Book borrowed successfully!");
}
catch (InvalidOperationException ex)
{
    ConsoleHelper.WriteError($"Error: {ex.Message}");
}
```

## 📊 Business Rules & Logic

### **Member Type Privileges**
| Member Type | Max Books | Loan Period |
|-------------|-----------|-------------|
| Student     | 3 books   | 14 days     |
| Teacher     | 10 books  | 30 days     |
| Staff       | 5 books   | 21 days     |
| External    | 2 books   | 7 days      |

### **Fine Calculation**
- **Rate**: ₺0.50 per day for overdue books
- **Automatic Calculation**: Applied when book is returned late
- **Member Tracking**: Total fines accumulated per member

### **Validation Rules**
- **ISBN Uniqueness**: No duplicate ISBN numbers allowed
- **Email Uniqueness**: One email per member
- **Membership Numbers**: Auto-generated unique identifiers
- **Book Availability**: Only available books can be borrowed
- **Loan Limits**: Enforced based on member type

## 💾 Data Persistence

### **JSON Storage**
The application uses JSON files for data persistence:
- `books.json` - Book collection data
- `members.json` - Member information
- `loans.json` - Loan transaction records

### **Sample Data Structure**
```json
{
  "books": [
    {
      "Id": 1,
      "Title": "C# Programming",
      "ISBN": "978-1234567890",
      "Author": {
        "FirstName": "John",
        "LastName": "Doe"
      },
      "Status": "Available",
      "Category": "Programming"
    }
  ]
}
```

## 🎨 User Interface Features

### **Colored Console Output**
- ✅ **Green**: Success messages and confirmations
- ❌ **Red**: Error messages and warnings  
- ⚠️ **Yellow**: Warning and information messages
- 📊 **Formatted Tables**: Clean data presentation

### **Interactive Menus**
- Numbered selection system
- Input validation and error handling
- Clear navigation paths
- "Press any key to continue" prompts

## 📈 Performance Features

- **Efficient LINQ Queries**: Optimized data filtering and searching
- **Memory Management**: Proper object disposal and cleanup
- **Lazy Loading**: Data loaded only when needed
- **Caching**: In-memory storage for session duration

## 🔄 Extensibility

The architecture supports easy extension:
- **New Entity Types**: Add new models inheriting from `BaseEntity`
- **Additional Repositories**: Implement `IRepository<T>` interface
- **New Business Rules**: Extend service layer methods
- **UI Enhancements**: Modify `ConsoleHelper` and menu systems

## 🎓 Educational Value

This project demonstrates proficiency in:
- **C# Language Features**: Modern syntax and capabilities
- **Object-Oriented Design**: Proper class hierarchies and relationships
- **Software Architecture**: Clean separation of concerns
- **Design Patterns**: Industry-standard implementation patterns
- **Best Practices**: Code organization and maintainability
- **Problem Solving**: Real-world business logic implementation

## 🚀 Future Enhancements

### **Planned Features:**
- [ ] Database integration (Entity Framework Core)
- [ ] Web API endpoints (ASP.NET Core)
- [ ] Authentication and authorization
- [ ] Email notification system
- [ ] Book reservation system
- [ ] Barcode scanning integration
- [ ] Export functionality (PDF, Excel)
- [ ] Advanced search filters
- [ ] Multi-language support

### **Technical Improvements:**
- [ ] Unit testing implementation
- [ ] Logging framework integration
- [ ] Configuration management
- [ ] Dependency injection container
- [ ] Performance monitoring
- [ ] Error tracking and reporting

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### **Development Guidelines:**
1. Follow C# coding standards and conventions
2. Maintain SOLID principles in design
3. Write meaningful commit messages
4. Update documentation for new features
5. Test thoroughly before submitting

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- Microsoft .NET documentation and best practices
- C# programming community for inspiration and guidance
- Design Patterns: Elements of Reusable Object-Oriented Software

## 📞 Contact

**Yiğit Ali Sunal** - [yigitalisunal03@gmail.com](mailto:yigitalisunal03@gmail.com)

---

### 🌟 If you found this project helpful, please give it a star!

**Made with ❤️ using C# and .NET - A comprehensive demonstration of modern software development practices**
