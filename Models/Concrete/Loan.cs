using LibraryManagementSystem.Models.Abstract;
using LibraryManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Concrete
{
    public class Loan : BaseEntitiy
    {
        public Member Member { get; set; }
        public Book Book { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal Fine { get; set; }
        public LoanStatus Status { get; set; }

        public Loan()
        {
            LoanDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(14); // 2 hafta  
            Status = LoanStatus.Active; // Başlangıçta aktif olarak ayarlanır
        }

        public bool IsOverdue => DateTime.Now > DueDate && Status == LoanStatus.Active;

        public decimal CalculateFine()
        {
            if (!IsOverdue) return 0;

            var overdueDays = (DateTime.Now - DueDate).Days;
            return overdueDays * 5.50m; // Gecikme başına 5.50 TL ceza
        }
    }
}
