using BookByte.Models;

namespace BookByte.Data
{
    // Оголошення інтерфейсу IBookByteRepo
    public interface IBookByteRepo 
    {
        // Метод для збереження змін в базі даних
        bool SaveChanges();
        
        // Метод для отримання всіх об'єктів типу BookBytes з бази даних
        IEnumerable<BookBytes> GetAllBookByte();
        // Метод для створення нового об'єкта типу BookBytes в базі даних
        void CreateBookByte(BookBytes bookBytes);
        // Метод для перевірки наявності об'єкта типу BookBytes за його ідентифікатором
        bool BookByteExits(int bookbyteId);
        // Метод для перевірки наявності зовнішнього об'єкта типу BookBytes за його ідентифікатором
        bool ExternalBookByteExist(int externalBookBytesId);

         // Метод для отримання всіх глав (Chapter) пов'язаних з об'єктом типу BookBytes
        IEnumerable<Chapter> GetChapterForBookByte(int bookbyteId);
        // Метод для отримання конкретної глави (Chapter) для об'єкта типу BookBytes
        Chapter GetChapter(int bookbyteId , int chapterId);
        // Метод для створення нової глави (Chapter) для об'єкта типу BookBytes
        void CreateChapter(int bookbyteId, Chapter chapter);
    }
}