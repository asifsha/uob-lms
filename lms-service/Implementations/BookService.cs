using System;
using AutoMapper;
using lms_data;
using lms_data.Entities;
using lms_data.Repositories;
using lms_service.Interfaces;
using lms_service.Models;


namespace lms_service.Implementations
{
    public class BookService : IBookService
    {
        private readonly IRepository<BookEntity> _repo;
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;


        public BookService(IRepository<BookEntity> repo, LibraryContext context, IMapper mapper)
        {
            _repo = repo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Book> CreateAsync(Book book)
        {
            var entity =  _mapper.Map<BookEntity>(book);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<Book>(entity);
        }


        public async Task DeleteAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return;
            _repo.Remove(e);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();
            if (books == null) return null;
            return _mapper.Map<IEnumerable<Book>>(books);
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            return _mapper.Map<Book>(book);
        }

        public async Task UpdateAsync(int id, Book book)
        {
            var entity = _mapper.Map<BookEntity>(book);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }
    }
}

