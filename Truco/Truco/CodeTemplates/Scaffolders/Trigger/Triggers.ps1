[T4Scaffolding.Scaffolder(Description = "Enter a description of Triggers here")][CmdletBinding()]
param(        
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string]$Template = "Empty",
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

@("ConexaoInsert", "ConexaoTriggerDelete", "ConexaoTriggerInsert", "ConexaoTriggerUpdate") | %{
	Scaffold Trigger $ModelType -Template $_ -Force:$Force
}