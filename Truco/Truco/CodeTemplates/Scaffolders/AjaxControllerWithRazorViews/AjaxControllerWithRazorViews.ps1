[T4Scaffolding.Scaffolder(Description = "Enter a description of AjaxControllerWithRazorViews here")][CmdletBinding()]
param(        
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$ControllerName,   
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 1)][string]$FkName,    
	[string]$ModelType,
	[string]$Area,
	[string]$DbContextType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Scaffold AjaxPartialViewModel $ControllerName $FkName -ModelType $ModelType -Force:$Force -Area:$Area 

Scaffold AjaxController $ControllerName $FkName -ModelType $ModelType -Force:$Force -Area:$Area -DbContextType:$DbContextType

Scaffold AjaxRazorViews $ControllerName $FkName -ModelType $ModelType -Force:$Force -Area:$Area 