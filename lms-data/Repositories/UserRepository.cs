using lms_data.Entities;
using lms_data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace lms_data.Repositories
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(LibraryContext context) : base(context) { }

        private IQueryable<UserEntity> BaseQuery()
        {
            return _dbSet;
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<bool> ExistsAsync(string username)
        {
            return await BaseQuery()
                .AnyAsync(x => x.Username == username);
        }
    }
}