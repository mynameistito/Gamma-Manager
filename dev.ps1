$ErrorActionPreference = "Stop"

$repositoryDirectory = $PSScriptRoot
$solutionPath = Join-Path $repositoryDirectory "Gamma Manager.sln"
$executablePath = Join-Path $repositoryDirectory "Gamma Manager\bin\Debug\Gamma Manager.exe"

$msbuildCandidates = @(
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuildPath = $msbuildCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $msbuildPath) {
    throw "MSBuild was not found. Install Visual Studio Build Tools with the .NET desktop build tools."
}

& $msbuildPath $solutionPath /t:Build /p:Configuration=Debug /p:SignAssembly=false /p:SignManifests=false /m
if ($LASTEXITCODE -ne 0) {
    throw "The Debug build failed."
}

Start-Process -FilePath $executablePath -WorkingDirectory (Split-Path $executablePath)
Write-Host "Gamma Manager is running from the Debug build."
