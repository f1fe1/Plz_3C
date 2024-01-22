using System.Linq;
using BookByte.Models;

namespace BookByte.Data
{
    public class BookByteRepo : IBookByteRepo
    {
        private readonly AppDbContext _context;

        public BookByteRepo(AppDbContext context)
        {
            _context = context;
        }

         // Перевіряє, чи існує об'єкт BookByte з вказаним ідентифікатором.
        public bool BookByteExits(int bookbyteId)
        {
            return _context.BookBytes.Any(p => p.Id == bookbyteId);
        }

        // Створює новий об'єкт BookBytes у базі даних.
        public void CreateBookByte(BookBytes bookBytes)
        {
            if(bookBytes == null)
            {
                throw new ArgumentNullException(nameof(bookBytes));
            }
            _context.BookBytes.Add(bookBytes);
        }

        // Створює новий об'єкт Chapter для певного BookByte у базі даних.
        public void CreateChapter(int bookbyteId, Chapter chapter)
        {
            if(chapter == null)
            {
                throw new ArgumentNullException(nameof(chapter));

            }
            
            chapter.BookByteId = bookbyteId;
            _context.Chapter.Add(chapter);
        }

        // Перевіряє, чи існує зовнішній BookByte з вказаним ISBN.
        public bool ExternalBookByteExist(int externalBookBytesId)
        {
            return _context.BookBytes.Any(p => p.ISBN == externalBookBytesId);
        }

        // Отримує всі об'єкти BookBytes у базі даних (не реалізовано).
        public IEnumerable<BookBytes> GetAllBookByte()
        {
            throw new NotImplementedException();
        }

        // Отримує об'єкт Chapter за вказаними ідентифікаторами.
        public Chapter GetChapter(int bookbyteId, int chapterId)
        {
            return _context.Chapter
                .Where(c => c.BookByteId == bookbyteId && c.Id == chapterId).FirstOrDefault();
        }

        // Отримує всі об'єкти Chapter для певного BookByte, впорядковані за назвою BookByte.
        public IEnumerable<Chapter> GetChapterForBookByte(int bookbyteId)
        {
            return _context.Chapter
                .Where(c => c.BookByteId == bookbyteId)
                .OrderBy(c => c.BookByte.Title);
        }

        // Зберігає всі зміни у базі даних.
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}