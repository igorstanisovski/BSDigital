# BSDigital
BTC/EUR Depth Chart

# How to start the app - Windows OS

## Setup - Prerequisites
- .NET SDK 10.0 or higher
- Node.js 20.0 or higher
- Docker Desktop
- Visual Studio 2026 (optional, for development)
- Visual Studio Code (optional, for Angular development)

## How to start

### Option 1: Using the startup script  
1. Open PowerShell in the root directory
2. Run the startup script:
```powershell
   .\build.ps1
```
3. The script will:
   - Check all prerequisites
   - Install npm packages (use `-SkipNpm` flag to skip: `.\build.ps1 -SkipNpm`)
   - Start Docker containers (PostgreSQL + .NET server)
   - Start Angular development server
4. Go to `http://localhost:4200` and start playing!

**Services:**
- Frontend: http://localhost:4200
- Backend API: http://localhost:5000
- PostgreSQL: localhost:5432

**To stop:**
- Press `Ctrl+C` to stop the Angular dev server
- Run `docker-compose down` to stop Docker containers

## How to run tests
1. Open PowerShell in the root directory
2. Run:
```powershell
   dotnet test
```

## Notes
- PostgreSQL runs in Docker with default credentials (username: `postgres`, password: `postgres`)
- Database migrations are applied automatically on startup in Development mode
- CORS is configured to allow `http://localhost:4200`