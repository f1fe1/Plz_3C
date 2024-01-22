using BookCollection.Dtos;

namespace BookCollection.AsyncDataServices
{
    //визначає інтерфейс з назвою IMessageBusClient
    public interface IMessageBusClient
    {
        //визначає метод інтерфейсу 
        //Метод не повертає жодного значення (void), тобто він не має результату.
        void PublishNewBook(BookPublishedDto bookPublishedDto);
    }
}