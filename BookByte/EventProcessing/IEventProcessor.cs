namespace BookByte.EventProcessing
{
    // Це визначення інтерфейсу IEventProcessor
    public interface IEventProcessor
    {
        //Цей інтерфейс має один метод ProcessEvent, який очікує один параметр типу string 
        //з іменем message. Метод не повертає значення (void)
        void ProcessEvent(string message);

        //цей код створює інтерфейс для обробки подій 
    }
}