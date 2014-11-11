$framework = "4.0"

properties {
	$base_dir = resolve-path .
	$source_dir = "$base_dir\src"
	$lib_dir = "$base_dir\lib"
	$tools_dir = "$base_dir\tools"
	$_dir = "$base_dir\build\"
	$build_dir = "$base_dir\build\"
	$bin_dir = "$build_dir\bin\\"
	$release_dir = "$base_dir\release"
	$packageinfo_dir = "$base_dir\packaging"
	$version = "5.0.0"
	$isbeta = $True
}

include .\tools\psake_ext.ps1
  
task default -depends clean, build, output, package, uploadpackages

task clean {
	rd $build_dir -recurse -force -erroraction silentlycontinue | out-null
	rd $release_dir -recurse -force -erroraction silentlycontinue | out-null
	md $release_dir -force -erroraction silentlycontinue | out-null
	
    Generate-Assembly-Info `
        -file "$source_dir\CommonAssemblyInfo.cs" `
        -title "NES $version" `
        -description "NES Framework for .NET" `
        -company "Elliot Ritchie" `
        -product "NES $version" `
        -version $version `
        -copyright "Copyright Elliot Ritchie 2014"
}

task build -depends clean {
	exec { msbuild "$source_dir\NES.sln" /t:clean /t:build /p:configuration=Release /v:quiet /p:outdir=$bin_dir }
	
}

task output -depends build {
	get-childitem $bin_dir -recurse -include NES* -exclude *Tests* | copy-item -destination $build_dir
}

task package -depends output {
	$spec_files = @(Get-ChildItem $source_dir -recurse -include NES*.nuspec)
	$nuget_version = $version
	if ($isbeta) {
		$nuget_version = "$version-beta"
	}
	
	foreach ($spec in $spec_files)
	{
	  $ProjectFileFullPath = [System.IO.Path]::ChangeExtension($spec.FullName,".csproj")
	& $tools_dir\nuget\NuGet.exe pack $ProjectFileFullPath -o $release_dir -Version $nuget_version -Symbols -IncludeReferencedProjects
	}
}

task uploadpackages -depends package {
	$apiKey = Read-Host 'Upload nuget packages. Api Key?'
	if ($apiKey) {
	  Write-Host "Upload now the nuget packages..."
	  $nuget_files = @(Get-ChildItem $release_dir)
	  cd $release_dir
	  if ($isbeta) {
		$nuget_version = "$version-beta"
	  }
	  foreach ($nuget_file in $nuget_files)
	  {
	    $NuGetPackageName = [System.IO.Path]::GetFileNameWithoutExtension($nuget_file)
		Write-Host "Delete package $NuGetPackageName with version $nuget_version" 
		& $tools_dir\nuget\NuGet.exe delete $NuGetPackageName $nuget_version $apiKey -NonInteractive
		
		$NuGetPackageName = [System.IO.Path]::GetFileName($nuget_file)
		Write-Host "Upload package $NuGetPackageName"
		& $tools_dir\nuget\NuGet.exe push $NuGetPackageName $apiKey
	  }
	}
}