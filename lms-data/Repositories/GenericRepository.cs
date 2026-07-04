using System;
using Microsoft.EntityFrameworkCore;

namespace lms_data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly LibraryContext _context;
        protected readonly DbSet<TEntity> _dbSet;


        public Repository(LibraryContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }


        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }


        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
