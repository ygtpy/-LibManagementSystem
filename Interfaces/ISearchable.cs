﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Interfaces
{
    internal interface ISearchable
    {
        bool MatchesSearchTerm(string searchTerm);
    }
}
