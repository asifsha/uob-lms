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

        public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        {
            var entities = await _borrowRecordRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BorrowRecord>>(entities);
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            var entity = await _borrowRecordRepository.GetBorrowRecordWithDetailsAsync(id);

            return entity == null
                ? null
                : _mapper.Map<BorrowRecord>(entity);
        }

        public async Task<BorrowRecord> BorrowBookAsync(int bookId, int memberId)
        {
            var book = await _bookService.GetByIdAsync(bookId);

            if (book == null)
                throw new InvalidOperationException("Book not found.");

            var member = await _memberService.GetByIdAsync(memberId);
            if (member == null)
                throw new InvalidOperationException("Member not found.");

            var active = await _borrowRecordRepository.GetActiveBorrowRecordsAsync();
            if (active.Any(x => x.BookId == bookId))
                throw new InvalidOperationException("Book is already borrowed.");

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

        public async Task<BorrowRecord?> ReturnBookAsync(int id)
        {
            var entity = await _borrowRecordRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            entity.ReturnDate = DateTime.UtcNow;

            _borrowRecordRepository.Update(entity);
            await _borrowRecordRepository.SaveChangesAsync();

            return _mapper.Map<BorrowRecord>(entity);
        }

        public async Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync()
        {
            var entities = await _borrowRecordRepository.GetOverdueRecordsAsync();
            return _mapper.Map<IEnumerable<BorrowRecord>>(entities);
        }
    }
}