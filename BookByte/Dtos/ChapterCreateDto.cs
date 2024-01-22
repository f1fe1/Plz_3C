using System.ComponentModel.DataAnnotations;

namespace BookByte.Dtos
{
    // представляє об'єкт для передачі даних при створенні нового розділу (глави) книги.
    public class ChapterCreateDto
    {
        [Required]//вказує, що це поле є обов'язковим
        public string Title { get; set; }//яка представляє заголовок нового розділу

        [Required]//вказує, що це поле є обов'язковим
        public string Content { get; set; } //нового розділу (глави) книги
    }
}