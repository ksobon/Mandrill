param($installPath, $toolsPath, $package, $project)

# Create a temp copy of EO.PowerTools.dll. This temp
# copy will be automatically deleted when system reboot

$dllName = [System.IO.Path]::Combine($toolsPath, "EO.PowerTools.dll")
$tempFileName = [System.IO.Path]::GetTempFileName()
$tempDllName = $tempFileName + ".dll"
Copy-Item $dllName $tempDllName
Remove-Item $tempFileName

# Import EO.PowerTools.dll
Import-Module -name ($tempDllName)