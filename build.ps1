dotnet run --configuration "Release" --project "$PSScriptRoot/build/Build.csproj" -- $args
#  Start-Process -FilePath 'dotnet' -Wait -WorkingDirectory $PSScriptRoot -ArgumentList ("run --project build/Build.csproj -- " + $($args))
exit $LASTEXITCODE;