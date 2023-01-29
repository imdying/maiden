$ErrorActionPreference = "Stop";

# Reserved variables
$BuildNumber    ??= 0;
$BuildVersion   ??= '0.0.0';
$BuildVersionId ??= 'gold';

# Build info.
Echo @("Date    $(Get-Date)"
       "Build   $BuildNumber"
       "Version $BuildVersion`n"
       "Building...`n");

# User code
Write-Host "Hello World";