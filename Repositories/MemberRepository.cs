using LibraryManagementSystem.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories
{
    public class MemberRepository : BaseRepository<Member>
    {
        public MemberRepository() : base("members.json") { }

        public override async Task<List<Member>> SearchAsync(string searchTerm)
        {
            return _entities.Where(m => m.IsActive &&
                (m.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 m.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 m.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||  
                 m.MembershipNumber.Contains(searchTerm) ||
                 m.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task<Member> GetByMembershipNumberAsync (string membershipNumber)
        {
            return _entities.FirstOrDefault(m => m.IsActive &&
                                            m.MembershipNumber == membershipNumber);
        }
    }
}
