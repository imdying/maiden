$ErrorActionPreference = "Stop";

# Reserved variables
$BuildNumber    ??= 0;
$BuildVersion   ??= '0.0.0';
$BuildVersionId ??= 'Gold';

# Build info.
Echo @("Date    $(Get-Date)"
       "Build   $BuildNumber"
       "Version $BuildVersion`n"
       "Building...`n");

# User variables
$cwd = $pwd.Path;
$Project = 'Raiden.sln';

# User code
dotnet test @("$Project"
              '-s'
              './.runsettings');

if ($LASTEXITCODE -ne 0) {
    throw;
}

dotnet pack @("$Project"
              "/p:Version=$BuildVersion"
              '/p:Configuration=Release');

if ($LASTEXITCODE -ne 0) {
    throw;
}

# dotnet push