using AutoMapper;
using lms_data.Entities;
using lms_data.Repositories;
using lms_service.Interfaces;
using lms_service.Models;

namespace lms_service.Implementations
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public BorrowService(
            IBorrowRecordRepository borrowRecordRepository,
            IBookService bookService,
            IMemberService memberService,
            IMapper mapper)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _bookService = bookService;
            _memberService = memberService;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        {
            var entities = await _borrowRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowRecord>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid borrow record id.");

            var entity = await _borrowRecordRepository.GetBorrowRecordWithDetailsAsync(id);

            return entity == null
                ? null
                : _mapper.Map<BorrowRecord>(entity);
        }

        // =========================
        // BORROW BOOK
        // =========================
        public async Task<BorrowRecord> BorrowBookAsync(int bookId, int memberId)
        {
            ValidateIds(bookId, memberId);

            var book = await _bookService.GetByIdAsync(bookId);
            if (book == null)
                throw new InvalidOperationException("Book not found.");

            var member = await _memberService.GetByIdAsync(memberId);
            if (member == null)
                throw new InvalidOperationException("Member not found.");

            // IMPORTANT BUSINESS RULE (was missing)
            if (!book.IsAvailable)
                throw new InvalidOperationException("Book is currently not available.");

            var activeBorrows = await _borrowRecordRepository.GetActiveBorrowRecordsAsync();

            if (activeBorrows.Any(x => x.BookId == bookId))
                throw new InvalidOperationException("Book is already borrowed.");

            // Optional rule: prevent too many active borrows per member
            var memberActiveBorrows = activeBorrows.Count(x => x.MemberId == memberId);
            if (memberActiveBorrows >= 5)
                throw new InvalidOperationException("Member has reached borrow limit (5 books).");

            var entity = new BorrowRecordEntity
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14)
            };

            await _borrowRecordRepository.AddAsync(entity);
            await _borrowRecordRepository.SaveChangesAsync();

            return _mapper.Map<BorrowRecord>(entity);
        }

        // =========================
        // RETURN BOOK
        // =========================
        public async Task<BorrowRecord?> ReturnBookAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid borrow record id.");

            var entity = await _borrowRecordRepository.GetByIdAsync(id);

            if (entity == null)
                throw new InvalidOperationException("Borrow record not found.");

            if (entity.ReturnDate != null)
                throw new InvalidOperationException("Book is already returned.");

            entity.ReturnDate = DateTime.UtcNow;

            _borrowRecordRepository.Update(entity);
            await _borrowRecordRepository.SaveChangesAsync();

            return _mapper.Map<BorrowRecord>(entity);
        }

        // =========================
        // OVERDUE
        // =========================
        public async Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync()
        {
            var entities = await _borrowRecordRepository.GetOverdueRecordsAsync();
            return _mapper.Map<IEnumerable<BorrowRecord>>(entities);
        }

        // =========================
        // VALIDATION HELPERS
        // =========================
        private void ValidateIds(int bookId, int memberId)
        {
            if (bookId <= 0)
                throw new InvalidOperationException("Invalid book id.");

            if (memberId <= 0)
                throw new InvalidOperationException("Invalid member id.");
        }
    }
}