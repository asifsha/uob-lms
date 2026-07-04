using System;
namespace lms_data.Entities
{
    public class LibraryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }


        public ICollection<BookEntity> Books { get; set; }
    }
}

