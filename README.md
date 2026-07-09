# interview-question-087

โปรเจกต์ตัวอย่างระบบจัดการรหัสสินค้า (Barcode Code 39) ตามข้อกำหนดในโจทย์

## โครงสร้าง
```
backend/ProductApi/     -> C# ASP.NET Core Web API
frontend/                -> Angular 18 (standalone components)
```

## เงื่อนไขที่ implement แล้ว
1. ปุ่ม ADD เพิ่มรหัสสินค้าลงตาราง (validate ทั้ง Front-end และ Back-end)
   - ตัวเลข + ตัวอักษรอังกฤษพิมพ์ใหญ่เท่านั้น
   - ความยาว 16 หลัก (ไม่นับขีด)
   - รูปแบบ xxxx-xxxx-xxxx-xxxx
2. ปุ่มลบ แสดง Confirm box ก่อนลบจริง
3. บาร์โค้ดมาตรฐาน Code 39 (ใช้ไลบรารี jsbarcode)
4. โครงสร้างฐานข้อมูล: ตาราง Products (Id, ProductCode unique, CreatedDate)

## วิธีรัน Backend
```bash
cd backend/ProductApi
dotnet restore
dotnet run
```
ค่าเริ่มต้นจะรันที่ http://localhost:5000 (หรือดู port จริงจาก console แล้วแก้ที่ frontend/src/environments/environment.ts ด้วยก่อนรัน Frontend)

Swagger UI: http://localhost:5000/swagger

หมายเหตุ: ตอนนี้ใช้ InMemory Database เพื่อให้รันทดสอบได้ทันที
หากต้องการต่อ SQL Server จริง ให้แก้ Program.cs:
```csharp
options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
```
และเพิ่ม connection string ใน appsettings.json

## วิธีรัน Frontend
```bash
cd frontend
npm install
npm start
```

เปิดที่ http://localhost:4200

## การตั้งชื่อ Repository / Package
- ตั้งชื่อ GitLab Repository เป็น: interview-question-087 (เปลี่ยนเลขให้ตรงกับที่ท่านได้รับ)
- ตั้งชื่อ namespace/package เป็น com.example.productapi (ฝั่ง backend) และ scope @example (ฝั่ง frontend) เพื่อให้ดูเหมือนทำงานภายใต้บริษัท example.com
