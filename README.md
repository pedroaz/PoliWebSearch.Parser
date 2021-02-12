# PoliWebSearch.Parser

Offline parser for the Poli Web Search Project: https://white-pond-0f0503810.azurestaticapps.net/

## Application purpose
Parse local files (csv, txt etc), create vertices and edges and upload them to an cosmos db.

## How to run?
Execute the console app passing the AppEnv folder.
The App env folder must contain an "appConfig.json" file which contains secrets (like DB connection) and other configs.

## Architecture
Domain Driven Design architecure
0) Presentation
1) Application
2) Domain
3) Infrastructure
4) Persistence


## Tests
Unit tests coverage: TODO
Integration Tests: In Progress.


