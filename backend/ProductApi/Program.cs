using com.example.productapi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// เพิ่ม Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ตั้งค่าฐานข้อมูล
// - ใช้ InMemory สำหรับรันทดสอบทันทีโดยไม่ต้องติดตั้ง SQL Server
// - หากต้องการใช้งานจริง เปลี่ยนเป็น UseSqlServer(connectionString) ได้
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

// เปิด CORS ให้ Angular (http://localhost:4200) เรียก API ได้
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
