using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Abstract
{
    public abstract class BaseEntitiy
    {
        public int Id {get; set;} 
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; } = true;

        protected BaseEntitiy()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
