using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntitiy
    {
        protected List<T> _entities = new List<T>();
        protected string _filePath;

        public BaseRepository(string fileName)
        {
            _filePath = Path.Combine("Data", fileName);
            LoadDataAsync().Wait();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return _entities.Where(e => e.IsActive).ToList();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.Id = _entities.Count > 0 ? _entities.Max(e => e.Id) + 1 : 1;
            entity.CreatedDate = DateTime.Now;
            _entities.Add(entity);
            await SaveDataAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            var existing = await GetByIdAsync(entity.Id);
            if (existing == null) return null;

            entity.UpdateDate = DateTime.Now;
            var index = _entities.IndexOf(existing);
            _entities[index] = entity;
            await SaveDataAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            entity.IsActive = false;
            await SaveDataAsync();
            return true;
        }

        public abstract Task<List<T>> SearchAsync(string searchTerm);

        protected async Task LoadDataAsync()
        {
            try
            {
                if (!Directory.Exists("Data"))
                    Directory.CreateDirectory("Data");

                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    _entities = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                _entities = new List<T>();
            }
        }

        protected async Task SaveDataAsync()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_entities, Formatting.Indented);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
    }    
}
