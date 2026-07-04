using System;
namespace lms_data.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;


        // Navigation
        public int? LibraryId { get; set; }
        public LibraryEntity Library { get; set; }
        public ICollection<BorrowRecordEntity> BorrowRecords { get; set; }
    }
}

