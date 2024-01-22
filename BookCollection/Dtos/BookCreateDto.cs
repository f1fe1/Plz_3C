using System.ComponentModel.DataAnnotations;

namespace BookCollection.Dtos
{
    //визначення об'єкта для передачі даних (DTO), який використовується для створення
    // або оновлення записів книг у додатку для зберігання колекції книг.
    public class BookCreateDto
    {

        [Required(ErrorMessage = "Назва книги є обов'язковою")]
        public string Title { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Ім'я автора є обов'язковим")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Рік видання є обов'язковим")]
        public int Year { get; set; }

        [MaxLength(100)]
        public string? Genre { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Власник є обов'язковим")]
        public string Owner { get; set; }
    }
}