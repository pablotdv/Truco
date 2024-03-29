[T4Scaffolding.ViewScaffolder("Razor", Description = "Adds an ASP.NET MVC view using the Razor view engine", IsRazorType = $true, LayoutPageFilter = "*.cshtml,*.vbhtml|*.cshtml,*.vbhtml")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Controller,
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 1)][string]$ViewName,
	[string]$ModelType,
	[string]$Template = "Empty",
	[string]$Area,
	[alias("MasterPage")]$Layout,	# If not set, we'll use the default layout
 	[string[]]$SectionNames,
	[string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$name = $Controller

# Ensure we have a controller name, plus a model type if specified
if ($ModelType) {
	$foundModelType = Get-ProjectType $ModelType -Project $Project
	if (!$foundModelType) { return }
	$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $Project)
}

# Decide where to put the output
$outputFolderName = Join-Path Views $Controller
if ($Area) {
	# We don't create areas here, so just ensure that if you specify one, it already exists
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputFolderName = Join-Path $areaPath $outputFolderName
}


if ($foundModelType) { $relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project) }
if (!$relatedEntities) { $relatedEntities = @() }



$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$areaNamespace = if ($Area) { [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + ".Areas.$Area") } else { $defaultNamespace }
$viewModelsNamespace = [T4Scaffolding.Namespaces]::Normalize($areaNamespace + ".ViewModels")

# Render the T4 template, adding the output to the Visual Studio project
$outputPath = Join-Path $outputFolderName $ViewName
Add-ProjectItemViaTemplate $outputPath -Template $Template -Model @{
	IsContentPage = [bool]$Layout;
	Layout = $Layout;
	Name = $name;
	SectionNames = $SectionNames;
	PrimarySectionName = $PrimarySectionName;
	ReferenceScriptLibraries = $ReferenceScriptLibraries.ToBool();
	ViewName = $ViewName;
	PrimaryKeyName = $primaryKeyName;
	ViewDataType = [MarshalByRefObject]$foundModelType;
	ViewDataTypeName = $foundModelType.Name;
	RelatedEntities = $relatedEntities;
	ModelType = [MarshalByRefObject]$foundModelType;
	DefaultNamespace = $defaultNamespace;
	ViewModelsNamespace = $viewModelsNamespace;
	ControllerName = $Controller;
	Area = $Area;
} -SuccessMessage "Added $ViewName view at '{0}'" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
