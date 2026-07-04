using System;
using lms_data;
using lms_service.Models;

namespace lms_service.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task UpdateAsync(int id, Book book);
        Task DeleteAsync(int id);
    }
}

