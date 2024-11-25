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
	[Target]@{ExeFileName='redis-server.exe'; RelativePath=".\..\..\..\Tools\Redis\"; Params="redis.windows.conf"}
	[Target]@{ExeFileName='etcd.exe'; RelativePath=".\..\..\..\Tools\etcd\"; Params=""}
	[Target]@{ExeFileName='etcdkeeper.exe'; RelativePath=".\..\..\..\Tools\etcdkeeper\"; Params="-p 9001"}
	[Target]@{ExeFileName='App.exe'; RelativePath=".\..\..\..\Bin\";}
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