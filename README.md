# Online Course Catalog API

## 📌 Overview
REST API for managing online courses, languages, and users.
Built using ASP.NET Core with clean architecture and soft delete implementation.

---

## 🛠 Tech Stack
- ASP.NET Core Web API
- .NET 8.0
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Role-based Authorization
- Soft Delete (DeletedAt)

---

## 📂 Project Structure
Controllers/      → API endpoints  
Models/           → Database entities  
DTOs/             → Data transfer objects  
Data/             → DbContext  
Middlewares/      → Custom middleware  
Helpers/          → Utility classes  
Responses/        → Standardized API responses  

---

## 🔐 Authentication
- JWT-based authentication
- Role: ADMIN, USER
- Protected endpoints use `[Authorize]`

---

## 🗑 Soft Delete
All entities use `DeletedAt` column.
Deleted data is excluded using Global Query Filter.

---

## 🚀 How to Run

1. Clone repository
2. Update connection string in `appsettings.json`
3. Navigate to the project directory
4. Run migration:

   `dotnet ef database update`

5. Run project:

   `dotnet watch`

6. Open Swagger (if enabled):

   http://localhost:{port}/swagger/index.html   
   http://localhost:{port}/{endpoint}

---

## 📌 API Endpoints

### 🧪 Default Seed Account

For testing:

Email: admin@gmail.com  
Password: 12345678  
Role: ADMIN 

### Authorization Header Example

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

### Auth

- GET /api/Auth -> ADMIN 
- GET /api/Auth/{id} -> ADMIN 
- POST /api/Auth/login -> PUBLIC
- POST /api/Auth/register -> PUBLIC
- PUT /api/Auth/update/{id} -> ADMIN
- DELETE /api/Auth/delete/{id} -> ADMIN 

### Courses

- GET /api/Courses -> PUBLIC
- GET /api/Courses/{id} -> PUBLIC
- POST /api/Courses/create -> ADMIN 
- PUT /api/Courses/update/{id} -> ADMIN 
- DELETE /api/Courses/delete/{id} -> ADMIN 

### Language

- GET /api/Language -> ADMIN/USER 
- GET /api/Language/{id} -> ADMIN/USER 
- POST /api/Language/create -> ADMIN 
- PUT /api/Language/update/{id} -> ADMIN 
- DELETE /api/Language/delete/{id} -> ADMIN 

### Topic

- GET /api/Topic -> ADMIN/USER
- GET /api/Topic/{id} -> ADMIN/USER   
- POST /api/Topic/create -> ADMIN  
- PUT /api/Topic/update/{id} -> ADMIN 
- DELETE /api/Topic/delete/{id} -> ADMIN 

### Response API

200 - Success  
201 - Created  
400 - Bad Request  
401 - Unauthorized  
403 - Forbidden  
404 - Not Found  
500 - Internal Server Error / Not Found / Salah Request Body

### Postman Collection 

Postman Collection sudah ada pada respository ini

### Note

- Untuk Request Body pada Method POST dan PUT ada pada masing2 endpoint yg ada di collection postman
- Untuk endpoint yg bukan PUBLIC harus menggunakan bearer token dari jwt yg telah digenerate pada saat login

*templatenya sudah ada pada collection postman*

---

## 👨‍💻 Author
Ikhsan Hadi