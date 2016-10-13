[T4Scaffolding.Scaffolder(Description = "Enter a description of Portugues.ViewModel here")][CmdletBinding()]
param(      
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ViewModelName,  
	[string]$ModelType,   
    [string]$Project,
	[string]$Area,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Interpret the "Force" and "ForceMode" options
$overwriteViewModel = $Force -and ((!$ForceMode) -or ($ForceMode -eq "ViewModelOnly"))
$overwriteFilesExceptViewModel = $Force -and ((!$ForceMode) -or ($ForceMode -eq "PreserveViewModel"))

# If you haven't specified a model type, we'll guess from the ViewModel name
if (!$ModelType) {
	if ($ViewModelName.EndsWith("ViewModel", [StringComparison]::OrdinalIgnoreCase)) {
		# If you've given "PeopleViewModel" as the full ViewModel name, we're looking for a model called People or Person
		$ModelType = [System.Text.RegularExpressions.Regex]::Replace($ViewModelName, "ViewModel$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
		$foundModelType = Get-ProjectType $ModelType -Project $Project -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -ErrorAction SilentlyContinue
		}
	} else {
		# If you've given "people" as the ViewModel name, we're looking for a model called People or Person, and the ViewModel will be PeopleViewModel
		$ModelType = $ViewModelName
		$foundModelType = Get-ProjectType $ModelType -Project $Project -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -ErrorAction SilentlyContinue
		}
		if ($foundModelType) {
			$ViewModelName = [string](Get-PluralizedWord $foundModelType.Name) + "ViewModel"
		}
	}
	if (!$foundModelType) { throw "Cannot find a model type corresponding to a ViewModel called '$ViewModelName'. Try supplying a -ModelType parameter value." }
} else {
	# If you have specified a model type
	$foundModelType = Get-ProjectType $ModelType -Project $Project
	if (!$foundModelType) { return }
	if (!$ViewModelName.EndsWith("ViewModel", [StringComparison]::OrdinalIgnoreCase)) {
		$ViewModelName = $ViewModelName + "ViewModel"
	}
}

$outputPath = Join-Path ViewModels $ViewModelName

if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath
}

$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$areaNamespace = if ($Area) { [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + ".Areas.$Area") } else { $defaultNamespace }
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$viewModelNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))

Add-ProjectItemViaTemplate $outputPath -Template Portugues.ViewModelTemplate `
	-Model @{ 
		ViewModelName = $ViewModelName;
		ModelType = [MarshalByRefObject]$foundModelType; 
		ViewModelNamespace = $viewModelNamespace;
		ModelTypeNamespace = $modelTypeNamespace; 	
		AreaNamespace = $areaNamespace; 	
	} -SuccessMessage "Added Portugues.ViewModel output at {0}" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force