$inputPath = "C:\GitHub\BikeTouringGIS\BikeTouringGIS\bin\Release"
$buildPath = "C:\GitHub\BikeTouringGIS\build"
$outputPath = "C:\GitHub\BikeTouringGIS\NuGet packaging"
[xml]$XmlDocument = Get-Content build\theRightDirection.BikeTouringGIS.nuspec
$version = $XmlDocument.package.metadata.version
Copy-Item -Path $inputPath\* -Exclude *.xml,*.pdb,*.licenses -Destination $buildPath\lib\net45 -Recurse -Force
Copy-Item -Path $inputPath\log4netconfiguration.xml -Destination $buildPath\lib\net45 -Force
.\nuget pack build\theRightDirection.BikeTouringGIS.nuspec -outputdirectory "build"
$nugetPath = Join-Path $env:USERPROFILE ".nuget\packages"
$squirrelPath = Join-Path $nugetPath "squirrel.windows\*\tools\Squirrel.exe" | Convert-Path | Select-Object -Last 1
& $squirrelPath --releasify $buildPath\BikeTouringGIS.$version.nupkg --no-msi --icon  $buildPath\BikeTouringGIS.ico  --setupIcon $buildPath\BikeTouringGIS.ico --releaseDir $outputPath
Start-Sleep -Seconds 20
remove-item -Path "$outputPath\BikeTouringGIS installer.exe" -Force -ErrorAction Ignore
Start-Sleep -Seconds 20
Rename-Item -Path $outputPath\Setup.exe "$outputPath\BikeTouringGIS installer.exe"