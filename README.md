# interview-question-087

โปรเจกต์ระบบจัดการรหัสสินค้า (Barcode Code 39)
## โครงสร้าง
```
backend/ProductApi/     -> C# ASP.NET Core Web API
frontend/                -> Angular 18 (standalone components)
```
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
