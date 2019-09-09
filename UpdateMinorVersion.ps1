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

#Increment Minor Version - Will reset all sub nodes
$avMinor = [Convert]::ToInt32($avMinor,10)+1
$fvMinor = [Convert]::ToInt32($fvMinor,10)+1
$avBuild = 0
$fvBuild = 0

#Put new version back into csproj (XML)
$xml.Project.PropertyGroup.AssemblyVersion = "$avMajor.$avMinor.$avBuild"
$xml.Project.PropertyGroup.FileVersion = "$fvMajor.$fvMinor.$fvBuild"

#Save csproj (XML)
$xml.Save($path)