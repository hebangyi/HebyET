class Target {
    [string] $ExeFileName 
    [string] $RelativePath
    [string] $Params
}

function StartProcess {
    param (
        [Target]$target
    )
    $appBasepath = $PSScriptRoot
    $appFullpath = $appBasepath + $target.RelativePath
    if ([string]::IsNullOrEmpty($target.Params)) {
        Start-Process -WorkingDirectory $appFullpath -FilePath $target.ExeFileName
    }
    else {
        Start-Process -WorkingDirectory $appFullpath -FilePath $target.ExeFileName -ArgumentList $target.Params
    }
}

$targets = @(
	[Target]@{ExeFileName='Tool.exe'; RelativePath=".\..\..\..\Bin\"; Params="--AppType Proto2CS"}
    )
	
if ($args[0].Equals('wt')) {
    $command = ' '
    $targets | ForEach-Object{
        $appFullpath = $PSScriptRoot +  $_.RelativePath
        $command += ('nt ' + ' --title ' + $_.ExeFileName.Substring(0, $_.ExeFileName.Length-4) + ' -d ' + $appFullpath + ' "' + $appFullpath + $_.ExeFileName + '" ' + $_.Params + ' ;')
    }
    Start-Process -FilePath 'wt' -ArgumentList $command
}
else
{ 
    $targets | ForEach-Object{
        StartProcess $_
        Start-Sleep -s 1
    }
}