# BSDigital
BTC/EUR Depth Chart

## How to start the app - Windows OS

### Setup - Prerequisites

Visual Studio 2026 (should be compatible also with VS2022)  
Visual Studio Code  
Docker Desktop  
PostgreSQL/pgAdmin

### How to start 

1. Start Docker Desktop
2. Load the BSDigital.slnx file into VS2026
3. Start the app BSDigital with profile Container (Dockerfile) <sup>[1]</sup>
4. Load the WebApp folder into VS Code
5. Run `npm install`
6. Run `ng serve`
7. Go to `localhost:4200` and start playing!

<sup>Notes: </sup>  
<sup>
[1] Postgres instance is running with default username and password, EF should create database if it doesn't exist
</sup>

### How to run tests

1. Open developer powershell in VS2026 and on root run `dotnet test`