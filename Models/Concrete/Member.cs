using LibraryManagementSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Concrete
{
    public class Member : Person
    {
        public string MembershipNumber { get; set; }
        public MemberType MemberType { get; set; }
        public DateTime MembershipDate { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public decimal TotalFines { get; set; }


        public Member()
        {
            MembershipDate = DateTime.Now;
            MembershipNumber = GenerateMembershipNumber();
        }

        private string GenerateMembershipNumber()
        {
            return $"LIB- {DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Member: {FullName} ({MembershipNumber}) - Type: {MemberType}");
        }
    }
}
