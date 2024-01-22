using System.Linq;
using BookCollection.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCollection.Data
{
    //для взаємодії з базою даних
    public static class PrepDb
    {
        //викликається для підготовки бази даних
        //вказує, чи потрібно застосовувати міграції у виробничому середовищі.
         public static void PrepPopulation(IApplicationBuilder app , bool isProd)
         {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() , isProd);
            }
         }

        //Цей метод приймає контекст бази даних 

         private static void SeedData(AppDbContext context, bool isProd)
         {
            
            //спробує застосувати міграції до бази даних за допомогою context.Database.Migrate()
             if(isProd)   
            {
                Console.WriteLine("--> Attempting to aplly mirations..");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            
            //він додає три записи про книги і зберігає їх в базі даних.
            if(!context.Book.Any())
            {
                Console.WriteLine("--> Seeding data...... ");

                context.Book.AddRange(
                    new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Fiction", Owner = "John Doe", Year = 1925 },
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Drama", Owner = "Jane Smith", Year = 1960 },
                    new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopian", Owner = "Alice Johnson", Year = 1949 }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have date");
            }
         }
    }
}

