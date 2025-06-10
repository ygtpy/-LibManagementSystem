using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Enums
{
    public enum BookStatus
    {
        Available,
        Borrowed,
        Reserved,
        Lost,
        UnderMaintenance
    }

    public enum MemberType
    {
        Student,
        Teacher,
        Staff,
        External
    }

    public enum LoanStatus
    {
        Active,
        Returned,
        Overdue,
        Lost
    }

}
