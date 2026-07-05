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

        // =========================
        // CREATE
        // =========================
        public async Task<Book> CreateAsync(Book book)
        {
            ValidateBook(book);

            await ValidateIsbnUnique(book.ISBN);

            var entity = _mapper.Map<BookEntity>(book);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<Book>(entity);
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();

            return _mapper.Map<IEnumerable<Book>>(books);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);

            if (book == null)
                throw new InvalidOperationException("Book not found.");

            return _mapper.Map<Book>(book);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(int id, Book book)
        {
            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
                throw new InvalidOperationException("Book not found.");

            ValidateBook(book);

            // keep original ID
            book.Id = id;

            _mapper.Map(book, existing);

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);

            if (book == null)
                throw new InvalidOperationException("Book not found.");

            _repo.Remove(book);
            await _repo.SaveChangesAsync();
        }

        // =========================
        // VALIDATION METHODS
        // =========================

        private void ValidateBook(Book book)
        {
            if (book == null)
                throw new InvalidOperationException("Book data is required.");

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new InvalidOperationException("Book title is required.");

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new InvalidOperationException("ISBN is required.");

            if (book.PublishedYear < 1500 || book.PublishedYear > DateTime.UtcNow.Year)
                throw new InvalidOperationException("Published year is invalid.");

        }

        private async Task ValidateIsbnUnique(string isbn)
        {
            var books = await _repo.GetAllAsync();

            if (books.Any(x => x.ISBN == isbn))
                throw new InvalidOperationException("ISBN already exists.");
        }
    }
}