$ErrorActionPreference = "Stop"

$installerDirectory = $PSScriptRoot
$repositoryDirectory = Split-Path -Parent $installerDirectory
$solutionPath = Join-Path $repositoryDirectory "Gamma Manager.sln"
$buildOutputDirectory = Join-Path $installerDirectory "build\Release"
$buildIntermediateDirectory = Join-Path $installerDirectory "build\obj\Release"

New-Item -ItemType Directory -Force -Path $buildOutputDirectory,$buildIntermediateDirectory | Out-Null

$msbuildCandidates = @(
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuildPath = $msbuildCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $msbuildPath) {
    throw "MSBuild was not found. Install Visual Studio Build Tools with the .NET desktop build tools."
}

& $msbuildPath $solutionPath /t:Rebuild /p:Configuration=Release /p:SignAssembly=false /p:SignManifests=false "/p:OutputPath=$buildOutputDirectory\" "/p:IntermediateOutputPath=$buildIntermediateDirectory\" /m
if ($LASTEXITCODE -ne 0) {
    throw "The Release build failed."
}

$isccCandidates = @(
    "${env:LOCALAPPDATA}\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 6\ISCC.exe"
)
$isccPath = $isccCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $isccPath) {
    throw "Inno Setup was not found. Install it with: winget install --id JRSoftware.InnoSetup --exact"
}

& $isccPath (Join-Path $installerDirectory "GammaManager.iss")
if ($LASTEXITCODE -ne 0) {
    throw "The installer build failed."
}

Write-Host "Installer created in $repositoryDirectory\artifacts"
