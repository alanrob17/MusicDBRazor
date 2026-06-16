# MusicDBRazor

A modern, responsive ASP.NET Core 10.0 Razor Pages application with a clean, separated Data Access Layer utilizing Entity Framework Core to manage and showcase an artists database.

---

## 🏗️ Project Architecture

The solution is split into two layered projects:

1. **`MusicDB`** (Web Application):
   - Built on **ASP.NET Core 10.0 Razor Pages**.
   - Handles the User Interface, PageModels, and Routing.
   - Contains highly styled, responsive CRUD pages for Artists (Index, Details, Edit, Create, Delete).
   - Features custom-styled components: sliding window pagination (15 buttons), search filtering, HTML biographies, and centered card views optimized for phone, tablet, and PC.

2. **`MusicDB.Data`** (Class Library):
   - Class library targeting **.NET 10.0**.
   - Contains the EF Core `MusicDbContext` and scaffolded POCO entities (`Artist`, `Record`, `Disc`, `Track`).
   - Keeps data access isolated from the web presentation layer.

---

## 🛠️ Tech Stack & Dependencies

*   **.NET 10.0 SDK**
*   **Entity Framework Core 10.0.9** (SqlServer, Design, Tools)
*   **Bootstrap 5.x** (for baseline utility layout support)
*   **Custom Vanilla CSS** (for consistent, premium gradient branding)

---

## 🚀 Getting Started

### 1. Connection String Setup
For security, `appsettings.json` and `appsettings.Development.json` are ignored in Git. You need to create an `appsettings.json` file inside the `MusicDB` directory with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MusicDb": "Server=localhost,11433;Database=MusicDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

### 2. Building the Project
Run the build command from the root solution folder:
```powershell
dotnet build
```

### 3. Running the Project
Navigate to the web project directory and run it:
```powershell
dotnet run --project MusicDB/MusicDB.csproj
```

---

## 📂 EF Core Scaffolding Command Reference
If the database schema changes, you can re-scaffold the entities into the `MusicDB.Data` project using the following command:

```powershell
dotnet ef dbcontext scaffold `
    "Server=localhost,11433;Database=MusicDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;" `
    Microsoft.EntityFrameworkCore.SqlServer `
    --project MusicDB.Data/MusicDB.Data.csproj `
    --startup-project MusicDB/MusicDB.csproj `
    --context MusicDbContext `
    --context-namespace MusicDB.Data `
    --namespace MusicDB.Data.Entities `
    --output-dir Entities `
    --context-dir . `
    --no-onconfiguring `
    --force
```
