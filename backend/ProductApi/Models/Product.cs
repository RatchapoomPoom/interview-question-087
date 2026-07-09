using System.ComponentModel.DataAnnotations;

namespace com.example.productapi.Models
{
    // โครงสร้างฐานข้อมูล: ตาราง Products
    // ออกแบบให้เก็บเฉพาะข้อมูลที่จำเป็น และให้ ProductCode เป็น unique key
    public class Product
    {
        [Key]
        public int Id { get; set; }

        // รหัสสินค้า รูปแบบ xxxx-xxxx-xxxx-xxxx (ตัวเลข + อังกฤษพิมพ์ใหญ่เท่านั้น, 16 หลัก)
        [Required]
        [MaxLength(19)] // 16 ตัวอักษร + ขีด 3 ตัว
        public string ProductCode { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
