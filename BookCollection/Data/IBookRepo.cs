using BookCollection.Models;
using System.Collections.Generic;

namespace BookCollection.Data
{
    //визначає контракт (абстрактний клас, який визначає поведінку) для репозиторію книг
    //Репозиторій в цьому контекст використовується для взаємодії з даними про книги.
    public interface IBookRepo
    {
        //збереження змін
        bool SaveChanges();

        //Метод, який отримує всі книги з колекції. 
        IEnumerable<Book> GetAllBook();
        // Метод, який отримує книгу за її ідентифікатором (id).
        Book GetBookById(int id);
        // Метод, який створює (додає) нову книгу до колекції.
        void CreateBook(Book book);
    }
}