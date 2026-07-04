using System;
namespace lms_data.Entities
{
    public class BorrowRecordEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        // Navigation
        public BookEntity Book { get; set; }
        public MemberEntity Member { get; set; }
    }
}

