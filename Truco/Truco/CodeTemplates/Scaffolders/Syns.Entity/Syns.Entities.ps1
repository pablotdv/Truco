[T4Scaffolding.Scaffolder(Description = "Enter a description of Syns.Entities here")][CmdletBinding()]
param(        
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$TabelaNova,
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 1)][string]$TabelaVelha,
    [string]$Project,
	[string]$CodeLanguage,
	[string]$Template = "Empty",
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

@("ConexaoTriggerDelete", "ConexaoTriggerInsert", "ConexaoTriggerUpdate") | %{
	Scaffold Trigger -TabelaNova $TabelaNova -TabelaVelha $TabelaVelha -Template $_ -Force:$Force
}