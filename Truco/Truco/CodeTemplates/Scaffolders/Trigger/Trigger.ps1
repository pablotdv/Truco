[T4Scaffolding.Scaffolder(Description = "Enter a description of Triggers here")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string]$Template = "Empty",
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

if ($ModelType) {
	$foundModelType = Get-ProjectType $ModelType -Project $Project
	if (!$foundModelType) { return }
	$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $Project)
}

$triggerName = $foundModelType.Name + $Template

$outputPath = $outputFolderName = Join-Path Triggers $triggerName  

Add-ProjectItemViaTemplate $outputPath -Template $Template `
	-Model @{ 
		ModelType = [MarshalByRefObject]$foundModelType;
		TriggerName = $triggerName;
		ModelType2 = $foundModelType
	} `
	-SuccessMessage "Added Triggers output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force