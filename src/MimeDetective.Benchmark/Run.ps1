#!/usr/bin/env pwsh

$PSNativeCommandUseErrorActionPreference = $true
$ErrorActionPreference = 'Stop'

cd $PSScriptRoot

# Remove Artifacts
$artefacts = ".\BenchmarkDotNet.Artifacts"
if (Test-Path $artefacts)
{
	Remove-Item $artefacts -Recurse
}

# Run benchmarks
& dotnet restore
& dotnet build -c Release --no-restore
& dotnet run -c Release --no-build --framework net8.0
