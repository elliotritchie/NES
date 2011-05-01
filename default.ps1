$framework = "4.0"

properties {
	$base_dir = resolve-path .
	$source_dir = "$base_dir\src"
	$lib_dir = "$base_dir\lib"
	$tools_dir = "$base_dir\tools"
	$_dir = "$base_dir\build\"
	$build_dir = "$base_dir\build\"
	$bin_dir = "$build_dir\bin\"
}

task default -depends clean, build, output

task clean {
	rd $build_dir -recurse -force -erroraction silentlycontinue | out-null
	
}

task build -depends clean {
	exec { msbuild "$source_dir\NES.sln" /t:clean /t:build /p:configuration=Release /v:quiet /p:outdir=$bin_dir }
	
}

task output -depends build {
	get-childitem $bin_dir -recurse -include @('NES.dll', 'NES.pdb', 'NES.xml') | copy-item -destination $build_dir
}