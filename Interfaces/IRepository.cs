using LibraryManagementSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Interfaces
{
    public interface IRepository<T> where T : BaseEntitiy
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task <bool> DeleteAsync (int id);
        Task<List<T>> SearchAsync(string searchTerm);
    }
}
