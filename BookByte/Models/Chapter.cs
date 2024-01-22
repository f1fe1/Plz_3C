using System.ComponentModel.DataAnnotations;
using BookByte.Models;

namespace BookByte.Models
{
    // Оголошення класу Chapter
    public class Chapter
    {
        [Key]//позначає, що властивість Id є первинним ключем в базі даних.
        [Required]//вказує, що значення цієї властивості є обов'язковим для заповнення.
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int BookByteId { get; set; }

        // Властивість BookByte вказує на об'єкт BookBytes, пов'язаний з цією главою.
        public BookBytes BookByte { get; set; }
    }
}