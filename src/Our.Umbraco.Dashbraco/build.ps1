$SourceDir = "$PSScriptRoot/wwwroot/App_Plugins/Our.Umbraco.Dashbraco"
$TargetDir = "$PSScriptRoot/../Our.Umbraco.Dashbraco.Test/App_Plugins/Our.Umbraco.Dashbraco"

if (!(Test-Path -Path $SourceDir)) {
    Write-Host "The source directory $SourceDir does not exist. Please check the path and try again." -ForegroundColor Red
    exit 1
}

if (!(Test-Path -Path $TargetDir)) {
    New-Item -ItemType Directory -Path $TargetDir -Force
}

Write-Host "Cleaning up old files in $TargetDir"
Remove-Item -Path "$TargetDir/*" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "Copying files from $SourceDir to $TargetDir"
Copy-Item -Path "$SourceDir/*" -Destination $TargetDir -Recurse -Force