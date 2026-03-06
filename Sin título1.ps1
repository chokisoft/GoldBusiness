param(
    [string]$RepoRoot = "F:\Documents\Visual Studio 18\Projects\GoldBusiness",
    [string]$ProjectRelative = "GoldBusiness.WebApi\GoldBusiness.WebApi.csproj",
    [string]$Configuration = "Release",
    [string]$PublishDirRelative = "publish",
    [string]$ZipPathRelative = "publish.zip",
    [string]$ResourceGroup = "rg-goldbusiness-dev",
    [string]$WebAppName = "goldbusinesswebapi-dev"
)

# Resolve and set location
$RepoRoot = (Resolve-Path $RepoRoot).Path
Set-Location $RepoRoot

$ProjectPath = Join-Path $RepoRoot $ProjectRelative
$PublishDir = Join-Path $RepoRoot $PublishDirRelative
$ZipPath = Join-Path $RepoRoot $ZipPathRelative

if (-not (Test-Path $ProjectPath)) { Write-Error "Project not found: $ProjectPath"; exit 1 }

# Build/publish the WebApi project only
dotnet restore $ProjectPath
if ($LASTEXITCODE -ne 0) { Write-Error "dotnet restore failed"; exit $LASTEXITCODE }

if (Test-Path $PublishDir) { Remove-Item -Recurse -Force $PublishDir }
dotnet publish $ProjectPath -c $Configuration -o $PublishDir
if ($LASTEXITCODE -ne 0) { Write-Error "dotnet publish failed"; exit $LASTEXITCODE }

# Verify publish output
if (-not (Test-Path $PublishDir)) { Write-Error "Publish directory not found: $PublishDir"; exit 1 }
$files = Get-ChildItem -Path $PublishDir -Recurse -File
if ($files.Count -eq 0) { Write-Error "Publish directory is empty"; exit 1 }
Write-Host "Publish contains $($files.Count) files. Size:" ( ($files | Measure-Object Length -Sum).Sum / 1MB ) "MB"

# Create ZIP with forward-slash entry names (fix for Kudu/rsync)
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
Add-Type -AssemblyName System.IO.Compression.FileSystem

try {
    # Open file stream for ZIP
    $zipStream = [System.IO.File]::Open($ZipPath, [System.IO.FileMode]::Create)
    $zipArchive = New-Object System.IO.Compression.ZipArchive($zipStream, [System.IO.Compression.ZipArchiveMode]::Create, $false)

    foreach ($file in $files) {
        # compute relative path and normalize to forward slash
        $relative = $file.FullName.Substring($PublishDir.Length).TrimStart('\','/')
        $entryName = $relative -replace '\\','/'

        # create entry and copy contents
        $entry = $zipArchive.CreateEntry($entryName, [System.IO.Compression.CompressionLevel]::Optimal)
        $entryStream = $entry.Open()
        $fs = [System.IO.File]::OpenRead($file.FullName)
        try {
            $fs.CopyTo($entryStream)
        } finally {
            $fs.Close()
            $entryStream.Close()
        }
    }

    $zipArchive.Dispose()
    $zipStream.Close()
} catch {
    Write-Error "Failed to create normalized ZIP: $($_.Exception.Message)"
    if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
    exit 1
}

if (-not (Test-Path $ZipPath)) { Write-Error "ZIP not created: $ZipPath"; exit 1 }
Write-Host "ZIP created with normalized entries: $ZipPath ($(Get-Item $ZipPath).Length / 1MB) MB"

# Azure login (use existing session or SP env vars)
if ($env:AZURE_CLIENT_ID -and $env:AZURE_CLIENT_SECRET -and $env:AZURE_TENANT_ID) {
    az login --service-principal -u $env:AZURE_CLIENT_ID -p $env:AZURE_CLIENT_SECRET --tenant $env:AZURE_TENANT_ID | Out-Null
} else {
    az account show > $null 2>&1
    if ($LASTEXITCODE -ne 0) { az login | Out-Null } else { Write-Host "Using existing az session." }
}

# Deploy using recommended command
Write-Host "Running: az webapp deploy --resource-group $ResourceGroup --name $WebAppName --src-path `"$ZipPath`" --type zip"
$azOutput = & az webapp deploy --resource-group $ResourceGroup --name $WebAppName --src-path $ZipPath --type zip 2>&1
$azExit = $LASTEXITCODE

Write-Host "`n--- AZ CLI OUTPUT ---`n"
Write-Host $azOutput
Write-Host "`n--- AZ CLI EXIT CODE: $azExit ---`n"

if ($azExit -ne 0) { Write-Error "az returned exit code $azExit"; exit $azExit }

Write-Host "Deployment finished successfully."