using BookCollection.Models;

using System.Linq;
using System.Collections.Generic;

namespace BookCollection.Data
{
    //реалізацією інтерфейсу IBookRepo
    public class BookRepo : IBookRepo
    {
        //приватне поле _context
        private readonly AppDbContext _context;
        //поле ініціалізується через конструктор класу
        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        //Цей метод додає новий об'єкт книги до контексту бази даних (_context). 
        //Перед додаванням перевіряється, чи переданий об'єкт книги не є null.
        public void CreateBook(Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.Book.Add(book);
        }

        //Цей метод повертає всі книги з бази даних у вигляді списку
        public IEnumerable<Book> GetAllBook()
        {
            return _context.Book.ToList();
        }

        //Цей метод повертає об'єкт книги з бази даних за заданим ідентифікатором.
        public Book GetBookById(int id)
        {
            return _context.Book.FirstOrDefault(p => p.Id == id);
        }

        //метод поки не реалізований 
        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
