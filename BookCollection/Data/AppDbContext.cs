using Microsoft.EntityFrameworkCore;
using BookCollection.Models;

namespace BookCollection.Data
{
    //для роботи з базою даних
    public class AppDbContext : DbContext
    {
        //Конструктор класу AppDbContext, який приймає параметр DbContextOptions<AppDbContext> 
        //і передає його базовому конструктору класу DbContext
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        //набір даних (DbSet) для класу Book
        public DbSet<Book> Book { get; set; }
    }
}