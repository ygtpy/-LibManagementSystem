using LibraryManagementSystem.Models.Concrete;
using LibraryManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories
{
    public class LoanRepository : BaseRepository<Loan>
    {
        public LoanRepository() : base("loans.json") { }

        public override async Task<List<Loan>> SearchAsync(string searchTerm)
        {
            return _entities.Where(l => l.IsActive &&
                (l.Member?.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                 l.Book?.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }

        public async Task<List<Loan>> GetOverdueLoanAsync()
        {
            return _entities.Where(l => l.IsActive && l.IsOverdue).ToList();
        }

        public async Task<List<Loan>> GetActiveLoansByMemberAsync(string membershipNumber)
        {
            return _entities.Where(l => l.IsActive &&
                                      l.Status == LoanStatus.Active &&
                                      l.Member.MembershipNumber == membershipNumber)
                             .ToList();
        }
    }
}
