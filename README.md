# WineHub – lærerudgave efter Opgave 4

Komplet referenceprojekt til sammenligning før Opgave 5.

## Indeholder
- Visual Studio solution / .NET 10 Web API
- EF Core 10 + SQL Server
- `WineHubDbContext`, relationskonfiguration og seed data
- async repositories
- Unit of Work / database transaction omkring `CreateOrder`
- server-side prisberegning
- Vue 3 + Vite
- xUnit
- integrationstest af rollback
- **ekstraopgave implementeret:** optimistic concurrency med SQL Server `rowversion` og test med to DbContexts

## Database
Standard er SQL Server LocalDB på Windows.

Kør:
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project WineHub.Api
dotnet ef database update --project WineHub.Api
```

Migrationen genereres lokalt, så den matcher præcis den EF Core 10-version, der restore's.

## Start
```
dotnet run --project WineHub.Api --launch-profile http
cd winehub-web
npm install
npm run dev
```

## Tests
```
dotnet test --filter "Category!=Integration"
dotnet test --filter "Category=Integration"
```
Integrationstests kræver LocalDB.

## Arkitektur
Vue -> Controller -> Manager -> Services -> Repository interfaces -> EF repositories -> DbContext -> SQL Server

Manageren ejer use-case-orkestreringen. `StockService` ejer lagerregler. `EfUnitOfWork` ejer transaktionsmekanikken.

## Bro til Opgave 5
Næste trin er Identity Provider + JWT + authentication/authorization.
