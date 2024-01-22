namespace BookByte.Dtos
{
    //Цей клас містить дані про читаний розділ (главу) книги.
    public class ChapterReadDto
    {
        public int Id { get; set; } //Представляє унікальний ідентифікатор для читаного розділу.
        public string Title { get; set; }//Представляє назву читаного розділу.
        public string Content { get; set; } //Представляє вміст читаного розділу (текст глави).
        public int BookByteId { get; set; } //Представляє ідентифікатор книги, до якої належить цей розділ.
    }
}