using System.ComponentModel.DataAnnotations;

namespace com.example.productapi.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string ProductCode { get; set; } = string.Empty;
    }

    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
