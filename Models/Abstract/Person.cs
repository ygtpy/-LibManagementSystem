﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Abstract
{
    public abstract class Person : BaseEntitiy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public abstract void DisplayInfo();
    }
}
