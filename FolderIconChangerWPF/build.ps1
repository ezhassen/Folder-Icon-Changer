dotnet run --configuration "Release" --project "$PSScriptRoot/build/Build.csproj" -- $args
exit $LASTEXITCODE;