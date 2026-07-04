using System;
using lms_data.Entities;
using lms_service.Models;

namespace lms_service.Implementations
{
    public interface IBorrowService
    {

        Task<IEnumerable<BorrowRecord>> GetAllAsync();

        Task<BorrowRecord?> GetByIdAsync(int id);

        Task<BorrowRecord> BorrowBookAsync(int bookId, int memberId);

        Task<BorrowRecord?> ReturnBookAsync(int id);

        Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync();

    }
}

