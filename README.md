# 🤖 AI Destekli Proje Asistanı API

> ASP.NET Core Web API kullanılarak geliştirilen, JWT Authentication ile korunan ve Claude AI entegrasyonuna sahip katmanlı mimariye uygun örnek bir projedir.

---

## 📖 Proje Hakkında

Bu proje, kullanıcıların sisteme güvenli bir şekilde giriş yaptıktan sonra seçtikleri proje kapsamında yapay zekâ ile iletişim kurmasını sağlayan bir Web API uygulamasıdır.

Her proje için veritabanında farklı bir sistem promptu tanımlanabilir. Kullanıcı soru sorduğunda ilgili prompt okunur ve Claude API'ye gönderilerek cevap üretilir.

---

## 🚀 Temel Özellikler

- 🔐 JWT Authentication
- 🤖 Claude AI Entegrasyonu
- 🗄 SQL Server Veritabanı
- 📚 Entity Framework Core
- 🏗 Katmanlı Mimari
- 📦 Repository Pattern
- ⚙️ Dependency Injection
- 📄 Swagger Desteği
- 🔧 Options Pattern
- 🔄 Genişletilebilir AI Provider Yapısı

---
## 🔄 Proje Akışı

```text
Kullanıcı
    │
    ▼
POST /api/Auth/login
    │
    ▼
JWT Token
    │
    ▼
POST /api/Ai/ask
    │
    ▼
AiController
    │
    ▼
AiService
    │
    ▼
ProjectRepository
    │
    ▼
SQL Server
(Proje Promptu)
    │
    ▼
Claude API
    │
    ▼
Yapay Zekâ Cevabı
    │
    ▼
Kullanıcı
```
## 🏗 Katmanlı Mimari

Proje SOLID prensipleri dikkate alınarak katmanlı mimariye uygun şekilde geliştirilmiştir.

```text
Controllers
     │
     ▼
Services
     │
     ▼
Repositories
     │
     ▼
Entity Framework Core
     │
     ▼
SQL Server
```

Her katmanın yalnızca kendi sorumluluğunu yerine getirmesi hedeflenmiştir. Böylece uygulama daha okunabilir, sürdürülebilir ve geliştirilebilir hale gelmiştir.

## 📁 Proje Yapısı

```text
AiProjectAssistant
│
├── src
│   └── AiProjectAssistant.Api
│       ├── Controllers
│       ├── Data
│       ├── DTOs
│       ├── Entities
│       ├── Extensions
│       ├── Middleware
│       ├── Options
│       ├── Repositories
│       ├── Services
│       ├── Program.cs
│       └── appsettings.json
│
├── README.md
└── .gitignore
```