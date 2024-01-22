namespace BookCollection.Dtos
{
    //представляє об'єкт для передачі даних про книгу, яка була опублікована.
    public class BookPublishedDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Event { get; set; }
    }
}