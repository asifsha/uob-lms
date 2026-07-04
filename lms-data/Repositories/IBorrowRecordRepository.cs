using System;
using lms_data.Entities;

namespace lms_data.Repositories
{
    public interface IBorrowRecordRepository : IRepository<BorrowRecordEntity>
    {
        Task<IEnumerable<BorrowRecordEntity>> GetActiveBorrowRecordsAsync();
        Task<IEnumerable<BorrowRecordEntity>> GetOverdueRecordsAsync();
        Task<BorrowRecordEntity?> GetBorrowRecordWithDetailsAsync(int id);
    }
}

