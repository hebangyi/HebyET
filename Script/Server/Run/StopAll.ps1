class Target {
    [string] $ExeFileName 
    [string] $RelativePath
    [string] $Params
}

function EndProcess {
    param(
        $PName
    )
    $isActive = Get-Process -Name $PName -ErrorAction SilentlyContinue
    if($isActive){
        Stop-Process -Name $PName -Force
    }
}

$targets = @(
	[Target]@{ExeFileName='mongod.exe'; RelativePath=".\..\..\..\Tools\MongoDb\bin\"; Params=""}
	[Target]@{ExeFileName='redis-server.exe'; RelativePath=".\..\..\..\Tools\Redis\"; Params="redis.windows.conf"}
	[Target]@{ExeFileName='App.exe'; RelativePath=".\..\..\..\Bin\";}
    )

$targets | ForEach-Object{
    EndProcess $_.ExeFileName.Substring(0, $_.ExeFileName.LastIndexOf("."))
}

(Get-WmiObject Win32_Process -Filter "Name Like 'WindowsTerminal%' And CommandLine Like 'wt.exe nt  --title%'")|ForEach-Object{ Stop-Process -Id $_.ProcessId -Force }
