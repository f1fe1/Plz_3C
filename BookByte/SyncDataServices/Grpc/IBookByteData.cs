
using BookByte.Models;

namespace BookByte.SyncDataServices
{
    public interface IBookByteData
    {
        //визначає один метод ReturnAllBookByte(), який повертає перелік об'єктів BookBytes
        IEnumerable<BookBytes> ReturnAllBookByte();
    }
}