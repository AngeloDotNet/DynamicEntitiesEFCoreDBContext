## Pluginable EF Core template

Struttura:
- MyApp.Abstractions: contiene `IEntity` (marker interface).
- MyApp.Host: applicazione host che carica i plugin, registra le assembly per EF e fornisce `MyDbContext`.
- ExamplePlugin: esempio di plugin con entità `Product` e `ProductConfiguration`.
- scripts: `build-and-copy.ps1` e `build-and-copy.sh` per costruire e copiare i plugin nella cartella `plugins` del host.

Requisiti:
- .NET SDK 8 (i progetti target possono essere aggiornati)
- dotnet CLI

Pacchetti NuGet principali:
- McMaster.NETCore.Plugins (usato nel host per caricare plugin isolati)
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer (o altro provider)

Come usare (rapida):
1. Creare la solution e aggiungere i progetti (o semplicemente clonare questa struttura).
2. Dal root: `dotnet restore`
3. Costruire la solution e copiare plugin nella cartella output del host:
   - Windows PowerShell: `.\scripts\build-and-copy.ps1`
   - Linux/macOS: `./scripts/build-and-copy.sh`
   I file .dll dei plugin saranno copiati in `MyApp.Host/bin/Debug/net7.0/plugins` (modificabile con parametri).
4. Eseguire l'host
   - `dotnet run --project MyApp.Host`
5. Migrations
   - Esegui `dotnet ef migrations add Initial --project MyApp.Data --startup-project MyApp.Host`
   - Assicurati che i plugin siano copiati nella cartella `plugins` del progetto di startup (o imposta `PLUGIN_FOLDER` env var).

Note:
- I plugin devono referenziare la stessa assembly `MyApp.Abstractions` (shared) per far funzionare la marker interface.
- Il template fornisce sia script che un esempio di MSBuild target (opzionale) per copiare i plugin automaticamente.
- Personalizza i path, target framework e provider DB secondo le tue esigenze.