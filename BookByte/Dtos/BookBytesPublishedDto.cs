namespace BookByte.Dtos
{
    //Оголошує клас з назвою BookBytesPublishedDto
    public class BookBytesPublishedDto
    {
        //представляє унікальний ідентифікатор для книги
        public int Id {get; set;}
        //представляє назву книги
        public string Title { get; set; }
        //представляє подію або події, пов'язані з опублікованою книгою.
        public string Event {get; set;}
    }
}