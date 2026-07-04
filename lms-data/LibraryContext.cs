using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using lms_data.Entities;


namespace lms_data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }


        public DbSet<BookEntity> Books { get; set; }
        public DbSet<MemberEntity> Members { get; set; }
        public DbSet<LibraryEntity> Libraries { get; set; }
        public DbSet<BorrowRecordEntity> BorrowRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookEntity>(b =>
            {
                b.ToTable("Books");
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(250);
                b.Property(x => x.Author).HasMaxLength(200);
                b.Property(x => x.ISBN).HasMaxLength(50);
                b.HasIndex(x => x.ISBN).IsUnique(false);
                b.HasOne(x => x.Library).WithMany(l => l.Books).HasForeignKey(x => x.LibraryId).OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<MemberEntity>(m =>
            {
                m.ToTable("Members");
                m.HasKey(x => x.Id);
                m.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                m.Property(x => x.Email).HasMaxLength(200);
                m.HasIndex(x => x.Email).IsUnique(false);
            });


            modelBuilder.Entity<BorrowRecordEntity>(br =>
            {
                br.ToTable("BorrowRecords");
                br.HasKey(x => x.Id);
                br.HasOne(x => x.Book).WithMany(b => b.BorrowRecords).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
                br.HasOne(x => x.Member).WithMany(m => m.BorrowRecords).HasForeignKey(x => x.MemberId).OnDelete(DeleteBehavior.Cascade);
                br.Property(x => x.DueDate).IsRequired();
            });


            modelBuilder.Entity<LibraryEntity>(l =>
            {
                l.ToTable("Libraries");
                l.HasKey(x => x.Id);
                l.Property(x => x.Name).IsRequired().HasMaxLength(200);
            });


            // Seed minimal data
            modelBuilder.Entity<LibraryEntity>().HasData(new LibraryEntity { Id = 1, Name = "Central Library", Address = "123 Main St" });
        }
    }
}

