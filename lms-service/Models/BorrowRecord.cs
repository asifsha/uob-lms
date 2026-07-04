using System;
namespace lms_service.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Business navigation properties
        public Book? Book { get; set; }

        public Member? Member { get; set; }

        // Business helper properties
        public bool IsReturned => ReturnDate.HasValue;

        public bool IsOverdue =>
            !IsReturned && DateTime.UtcNow > DueDate;
    }
}

