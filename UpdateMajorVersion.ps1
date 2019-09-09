#Get Path to csproj
$path = "$PSScriptRoot\src\KE-PDC\KE-PDC.csproj"

#Read csproj (XML)
$xml = [xml](Get-Content $path)

#Retieve Version Nodes
$assemblyVersion = $xml.Project.PropertyGroup.AssemblyVersion
$fileVersion = $xml.Project.PropertyGroup.FileVersion

#Split the Version Numbers
$avMajor, $avMinor, $avBuild  = $assemblyVersion.Split(".")
$fvMajor, $fvMinor, $fvBuild = $fileVersion.Split(".")

#Increment Major Version - Will reset all sub nodes
$avMajor = [Convert]::ToInt32($avMajor,10)+1
$fvMajor = [Convert]::ToInt32($fvMajor,10)+1
$avMinor = 0
$fvMinor = 0
$avBuild = 0
$fvBuild = 0

#Put new version back into csproj (XML)
$xml.Project.PropertyGroup.AssemblyVersion = "$avMajor.$avMinor.$avBuild"
$xml.Project.PropertyGroup.FileVersion = "$fvMajor.$fvMinor.$fvBuild"

#Save csproj (XML)
$xml.Save($path)