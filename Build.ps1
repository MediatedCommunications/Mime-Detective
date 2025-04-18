#!/usr/bin/env pwsh

#Set-PSDebug -Trace 2
$PSNativeCommandUseErrorActionPreference = $true
$ErrorActionPreference = 'Stop'

echo "build: Build started"

Push-Location $PSScriptRoot

if(Test-Path .\artifacts) {
	echo "build: Cleaning .\artifacts"
	Remove-Item .\artifacts -Force -Recurse
}

Push-Location .\src\


& dotnet restore --no-cache
& dotnet build -c Release

$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]

$branch = ""
$revision = ""
$suffix = ""
$commitHash = ""
$buildSuffix = ""

echo "build: Package version suffix is $suffix"
echo "build: Build version suffix is $buildSuffix" 

foreach ($src in ls -Attributes Directory ./Mime*) {
    Push-Location $src

    echo "build: Packaging project in $src"

    if($suffix -ne "") {
        & dotnet pack -c Release --include-source --no-build -o ..\..\artifacts --version-suffix=$suffix
    } else {
        & dotnet pack -c Release --include-source --no-build -o ..\..\artifacts
    }
    if($LASTEXITCODE -ne 0) { exit 1 }    

    Pop-Location
}

foreach ($test in ls -Attributes Directory ./Mime*`(Tests`)) {
    Push-Location $test

	echo "build: Testing project in $test"

    & dotnet test -c Release
    if($LASTEXITCODE -ne 0) { exit 3 }

    Pop-Location
}

# foreach ($test in ls test/*.PerformanceTests) {
#     Push-Location $test

# 	echo "build: Building performance test project in $test"

#     & dotnet build -c Release
#     if($LASTEXITCODE -ne 0) { exit 2 }

#     Pop-Location
# }

Pop-Location
