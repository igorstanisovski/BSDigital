param(
    [switch]$SkipNpm
)

Write-Host "=== Application Startup Script ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host ".NET SDK found: $dotnetVersion" -ForegroundColor Green
    
    $requiredVersion = [Version]"10.0"
    $installedVersion = [Version]($dotnetVersion -split '-')[0]
    
    if ($installedVersion -lt $requiredVersion) {
        Write-Host ".NET SDK version $requiredVersion or higher is required" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host ".NET SDK not found. Please install .NET SDK 10.0 or higher" -ForegroundColor Red
    Write-Host "Download from: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

Write-Host "Checking Node.js..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "Node.js found: $nodeVersion" -ForegroundColor Green
    
    $requiredNodeVersion = [Version]"20.0"
    $installedNodeVersion = [Version]($nodeVersion -replace 'v', '' -split '-')[0]
    
    if ($installedNodeVersion -lt $requiredNodeVersion) {
        Write-Host "Node.js version 20.0 or higher is required" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host "Node.js not found. Please install Node.js 20.0 or higher" -ForegroundColor Red
    Write-Host "Download from: https://nodejs.org/" -ForegroundColor Yellow
    exit 1
}

Write-Host "Checking npm..." -ForegroundColor Yellow
try {
    $npmVersion = npm --version
    Write-Host "npm found: $npmVersion" -ForegroundColor Green
}
catch {
    Write-Host "npm not found" -ForegroundColor Red
    exit 1
}

Write-Host ""

Write-Host "Checking Angular CLI..." -ForegroundColor Yellow
try {
    $ngVersionOutput = ng version --json 2>$null | ConvertFrom-Json
    $installedNgVersion = [Version]$ngVersionOutput.cli.version

    Write-Host "Angular CLI found: $installedNgVersion" -ForegroundColor Green

    $requiredNgVersion = [Version]"17.0"

    if ($installedNgVersion -lt $requiredNgVersion) {
        Write-Host "Angular CLI version $requiredNgVersion or higher is required" -ForegroundColor Red
        Write-Host "Update with: npm install -g @angular/cli" -ForegroundColor Yellow
        exit 1
    }
}
catch {
    Write-Host "Angular CLI not found." -ForegroundColor Red
    Write-Host "Install with: npm install -g @angular/cli" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

Write-Host "Checking Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "Docker found: $dockerVersion" -ForegroundColor Green
}
catch {
    Write-Host "Docker not found. Please install Docker Desktop" -ForegroundColor Red
    Write-Host "Download from: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    exit 1
}

Write-Host "Checking if Docker is running..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "Docker is running" -ForegroundColor Green
}
catch {
    Write-Host "Docker is not running. Please start Docker Desktop" -ForegroundColor Red
    exit 1
}

Write-Host ""

if ($SkipNpm) {
    Write-Host "Skipping npm install (SkipNpm flag set)" -ForegroundColor Yellow
} else {
    Write-Host "Installing Angular app dependencies..." -ForegroundColor Yellow
    $webAppPath = Join-Path $PSScriptRoot "WebApp"

    if (-not (Test-Path $webAppPath)) {
        Write-Host "WebApp folder not found at: $webAppPath" -ForegroundColor Red
        exit 1
    }

    Push-Location $webAppPath

    npm install
    if ($LASTEXITCODE -ne 0) {
        Write-Host "npm install failed" -ForegroundColor Red
        Pop-Location
        exit 1
    }
    Write-Host "npm packages installed successfully" -ForegroundColor Green

    Pop-Location
}

Write-Host ""

Write-Host "Starting Docker containers..." -ForegroundColor Yellow

if (-not (Test-Path "docker-compose.yml")) {
    Write-Host "docker-compose.yml not found in root directory" -ForegroundColor Red
    exit 1
}

# Stop any existing containers
docker-compose down 2>$null

# Start containers
docker-compose up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to start Docker containers" -ForegroundColor Red
    exit 1
}

Write-Host "Docker containers started successfully" -ForegroundColor Green
Write-Host ""

Write-Host "Starting Angular development server..." -ForegroundColor Yellow
$webAppPath = Join-Path $PSScriptRoot "WebApp"

Push-Location $webAppPath

# Start ng serve in a new PowerShell window
Start-Process powershell `
  -ArgumentList "-NoExit", "-Command", "cd '$webAppPath'; ng.cmd serve"

Write-Host "✓ Angular dev server starting in new window..." -ForegroundColor Green
Write-Host "→ Angular app will be available at http://localhost:4200" -ForegroundColor Cyan

Pop-Location

Write-Host ""
Write-Host "=== All services started successfully! ===" -ForegroundColor Green
Write-Host "Backend API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "Frontend: http://localhost:4200" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the script (Docker containers will keep running)" -ForegroundColor Yellow
Write-Host "To stop all containers, run: docker-compose down" -ForegroundColor Yellow