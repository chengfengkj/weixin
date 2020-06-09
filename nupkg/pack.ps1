Param(
  [parameter(Mandatory=$false)][string]$repo="",
  [parameter(Mandatory=$false)][bool]$push=$false,
  [parameter(Mandatory=$false)][string]$apikey,
  [parameter(Mandatory=$false)][bool]$build=$true
)

# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder ".."


function Pack($projectFolder,$projectName) {
  Set-Location $projectFolder
  $releaseProjectFolder = (Join-Path $projectFolder "bin/Release")
  if (Test-Path $releaseProjectFolder)
  {
     Remove-Item -Force -Recurse $releaseProjectFolder
  }
  
   & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
   & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true
   if ($projectName) {
    $projectPackPath = Join-Path $releaseProjectFolder ("*.nupkg")
   }else {
    $projectPackPath = Join-Path $releaseProjectFolder ("*.nupkg")
   }
   
  # Write-Warning -Message (Join-Path $releaseProjectFolder ("*.nupkg"))
  # Write-Warning -Message ($packFolder)
   
   Move-Item -Force $projectPackPath $packFolder 
}

if ($build) {
  Set-Location $slnPath
  & dotnet restore WeiXin.sln

	Pack -projectFolder ("../weixin")


  Set-Location $packFolder
}

if($push) {
    if ([string]::IsNullOrEmpty($apikey)){
        Write-Warning -Message "未设置nuget仓库的APIKEY"
		exit 1
	}
	dotnet nuget push *.nupkg -s $repo -k $apikey
}