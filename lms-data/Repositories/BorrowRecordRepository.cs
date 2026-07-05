using lms_data.Entities;
using Microsoft.EntityFrameworkCore;

namespace lms_data.Repositories
{
    public class BorrowRecordRepository : Repository<BorrowRecordEntity>, IBorrowRecordRepository
    {
        public BorrowRecordRepository(LibraryContext context) : base(context) { }

        private IQueryable<BorrowRecordEntity> BaseQuery()
        {
            return _dbSet
                .Include(x => x.Book)
                .Include(x => x.Member);
        }

        public async Task<IEnumerable<BorrowRecordEntity>> GetActiveBorrowRecordsAsync()
        {
            return await BaseQuery()
                .Where(x => x.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRecordEntity>> GetOverdueRecordsAsync()
        {
            return await BaseQuery()
                .Where(x => x.ReturnDate == null && x.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<BorrowRecordEntity?> GetBorrowRecordWithDetailsAsync(int id)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}