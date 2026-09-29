# Migration
Generér den fulde EF Core migration lokalt:
dotnet ef migrations add InitialCreate --project WineHub.Api
dotnet ef database update --project WineHub.Api

Dette er bevidst: migrations er genereret kode og bør passe præcist til den installerede EF Core 10-version.
