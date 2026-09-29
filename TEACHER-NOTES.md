# Lærernoter
Sammenlign især: ansvar mellem Manager/Service/Repository, async hele vejen, transaktionsgrænse, server-side priser og testbarhed.

## Ekstraopgave
`StockItem.RowVersion` er `.IsRowVersion()`. To samtidige DbContexts kan derfor ikke begge gemme ud fra samme oprindelige version; den anden får `DbUpdateConcurrencyException`.

## Diskussion
EF Core `DbContext`/`DbSet` har allerede Unit-of-Work/Repository-lignende egenskaber. Egne repositories er her et bevidst undervisningsvalg, ikke en universel regel.
