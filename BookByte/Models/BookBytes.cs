using System.ComponentModel.DataAnnotations;

namespace BookByte.Models
{
    //// Оголошення класу BookBytes в просторі імен BookByte.Models
    public class BookBytes
    {
        [Key]//позначає властивість як первинний ключ в базі даних
        [Required]//вказує, що ця властивість обов'язкова для заповнення
        public int Id { get; set; }

        [Required]
        public int ISBN { get; set; } //ідентифікатор книги за стандартом ISBN

        [Required]
        public string Title { get; set; }

        //// Властивість Chapters - колекція глав книги
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
