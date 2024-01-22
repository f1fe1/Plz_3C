using BookCollection.Dtos;

namespace BookCollection.SyncDataServices.Http
{
    //Визначає інтерфейс з назвою IBookByteDataClient
    public interface IBookByteDataClient
    {
        //Оголошує метод із сигнатурою SendBookByteToChapter
        //Метод повертає об'єкт типу Task
        //він є асинхронним і може виконуватися в іншому потоці.
        Task SendBookByteToChapter(BookReadDto plat);
    }
}