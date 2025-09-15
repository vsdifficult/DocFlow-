# DocFlow- 


# 📄 DocFlow — Electronic Document Management System

A pet project built with **C# (.NET 9)** to demonstrate backend development skills.  
DocFlow is an electronic document management system that allows users to upload, approve, and manage documents with multi-step workflows.

---

## 🚀 Features
- 🔑 **Authentication & Authorization** (JWT, Role-Based Access Control)
- 📄 **Documents CRUD** with file attachments (PDF/DOCX)
- 🔄 **Multi-level approval workflow** (review → approve/reject)
- 📨 **Notifications** (Email, SignalR)
- 🕓 **Document versioning** with history
- 📜 **Audit logging** for all actions
- ⚡ **Caching** with Redis
- 🗄 **File storage** (MinIO/Azure Blob Storage)

---

## 🏗 Architecture
- **ASP.NET Core 9 (Web API)**
- **Clean Architecture** (API, Application, Domain, Infrastructure layers)
- **Entity Framework Core + PostgreSQL**
- **Redis** for caching workflow states
- **RabbitMQ** for asynchronous notifications
- **Docker Compose** for local development

---

## 🔧 Getting Started

### Clone the repository
```bash
git clone https://github.com/your-username/docflow-backend.git
cd docflow-backend

docker-compose up --build
API will be available at:
👉 http://localhost:5000/swagger


🧪 Testing
Unit tests: xUnit

Integration tests: Testcontainers

Run tests:

bash```
dotnet test
```

📂 API Documentation
Swagger/OpenAPI available at:
👉 http://localhost:5000/swagger/index.html
