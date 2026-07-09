using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using com.example.productapi.Data;
using com.example.productapi.DTOs;
using com.example.productapi.Models;
using System.Text.RegularExpressions;

namespace com.example.productapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        // ข้อ 1.1 + 1.2 + 1.3
        // - ตัวเลขและตัวอักษรภาษาอังกฤษพิมพ์ใหญ่เท่านั้น
        // - ความยาว 16 หลัก (ไม่นับขีด)
        // - รูปแบบ xxxx-xxxx-xxxx-xxxx
        private static readonly Regex ProductCodePattern =
            new(@"^[0-9A-Z]{4}-[0-9A-Z]{4}-[0-9A-Z]{4}-[0-9A-Z]{4}$", RegexOptions.Compiled);

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _db.Products
                .OrderBy(p => p.Id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    CreatedDate = p.CreatedDate
                })
                .ToListAsync();

            return Ok(products);
        }

        // POST api/products  -> ปุ่ม ADD
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
        {
            var code = (dto.ProductCode ?? string.Empty).Trim().ToUpperInvariant();

            if (!ProductCodePattern.IsMatch(code))
            {
                return BadRequest(new
                {
                    message = "รหัสสินค้าต้องเป็นตัวเลข/ตัวอักษรอังกฤษพิมพ์ใหญ่ รูปแบบ xxxx-xxxx-xxxx-xxxx (16 หลัก)"
                });
            }

            var exists = await _db.Products.AnyAsync(p => p.ProductCode == code);
            if (exists)
            {
                return Conflict(new { message = "รหัสสินค้านี้มีอยู่ในระบบแล้ว" });
            }

            var product = new Product
            {
                ProductCode = code,
                CreatedDate = DateTime.UtcNow
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var result = new ProductResponseDto
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                CreatedDate = product.CreatedDate
            };

            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, result);
        }

        // DELETE api/products/{id} -> ปุ่มลบ (Front-end จะเป็นคนแสดง Confirm box ก่อนเรียก endpoint นี้)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound(new { message = "ไม่พบรหัสสินค้าที่ต้องการลบ" });
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
