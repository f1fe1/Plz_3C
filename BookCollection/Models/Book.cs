using System.ComponentModel.DataAnnotations;
using System;

namespace BookCollection.Models
{

    //Оголошення класу Book, який представляє модель для книги.
 public class Book
    {
        [Key]//Атрибут, який позначає поле Id як ключове поле бази даних.
        [Required]//Атрибут, який вказує, що поле є обов'язковим для заповнення.
        public int Id { get; set; }

        [MaxLength(100)]//Атрибут, який обмежує максимальну довжину рядка
        //Атрибут, який вказує на обов'язковість заповнення поля
        //вказує власне повідомлення про помилку, яке буде виведено, якщо поле не буде заповнено.
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