# Remove Artifacts
$artefacts = ".\BenchmarkDotNet.Artifacts"
if (Test-Path $artefacts)
{
	Remove-Item $artefacts -Recurse
}

# Run benchmarks
dotnet run -c Release