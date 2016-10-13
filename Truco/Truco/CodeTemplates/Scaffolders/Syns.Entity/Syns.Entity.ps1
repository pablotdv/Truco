[T4Scaffolding.Scaffolder(Description = "Enter a description of Syns.Entity here")][CmdletBinding()]
param(        	
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Entidade,
	[String]$Tabela,
    [string]$Project,
	[string]$CodeLanguage,
	[string]$Template = "Entity",
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$outputPath = Join-Path Models $Entidade

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate $outputPath -Template $Template `
	-Model @{ 
		Namespace = $namespace; 
		Entidade = $Entidade;
		Tabela = $Tabela;
	} `
	-SuccessMessage "Added Syns.Entity output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force