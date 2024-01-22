namespace BookCollection.Dtos
{
    //використовувується для передачі даних про книгу між різними частинами програми
    public class BookReadDto
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string? Genre { get; set; }

        public string Owner { get; set; }
    }
}