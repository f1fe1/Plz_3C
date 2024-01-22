using BookByte.Models;
using Microsoft.EntityFrameworkCore;

namespace BookByte.Data
{
    //це підклас DbContext, який надається Entity Framework Core для взаємодії з базою даних.
    public class AppDbContext : DbContext
    {   
        //для налаштування контексту бази даних.
        public AppDbContext(DbContextOptions<AppDbContext> opt ) : base(opt)
        {
            
        }
        //представляють таблиці бази даних для класів BookBytes і Chapter відповідно.
        public DbSet<BookBytes> BookBytes { get; set; }
        public DbSet<Chapter> Chapter { get; set; }

        //Метод OnModelCreating викликається при створенні моделі бази даних і дозволяє налаштовувати відносини між таблицями.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Вказує, що налаштування будуть застосовані до сутності BookBytes.
            modelBuilder.Entity<BookBytes>()
                .HasMany(a => a.Chapters)//може бути багато об'єктів Chapter
                .WithOne(b => b.BookByte!)//має тільки один об'єкт BookByte.
                .HasForeignKey(b => b.BookByteId);//зовнішній ключ для відносин знаходиться в полі BookByteId таблиці Chapter.

            //Налаштування для сутності Chapter.
            modelBuilder
                .Entity<Chapter>()
                .HasOne(p => p.BookByte)// може бути тільки один об'єкт BookByte.
                .WithMany(p => p.Chapters!)//може мати багато об'єктів Chapter.
                .HasForeignKey(p =>p.BookByteId);// зовнішній ключ для відносин знаходиться в полі BookByteId таблиці Chapter.
        }
    }
}