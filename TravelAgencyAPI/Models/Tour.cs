using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models
{
    public class Tour
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string Name { get; set; } = String.Empty;
        [Required, MinLength(3)]
        public string Description { get; set; } = String.Empty;
        [Required, MinLength(3), MaxLength(100)]
        public string Country {  get; set; } = String.Empty;
        [Required, MinLength(3), MaxLength(100)]
        public string Type { get; set; } = String.Empty;
        [Required]
        public float Price { get; set;}
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
    }
}
