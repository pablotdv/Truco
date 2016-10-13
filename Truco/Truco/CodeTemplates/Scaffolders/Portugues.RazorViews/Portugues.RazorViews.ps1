[T4Scaffolding.Scaffolder(Description = "Adds ASP.NET MVC views for Create/Read/Update/Delete/Index scenarios")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Controller,
	[string]$ModelType,
	[string]$Area,
	[alias("MasterPage")]$Layout = "",	# If not set, we'll use the default layout
 	[alias("ContentPlaceholderIDs")][string[]]$SectionNames,
	[alias("PrimaryContentPlaceholderID")][string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[string]$ViewScaffolder = "Portugues.RazorView",
	[switch]$Force = $false,
	[switch]$Related = $false
)

@("Indice", "_Pesquisa", "_ParametrosPesquisa", "_CriarOuEditar", "Criar", "Editar", "Detalhes", "Excluir", "_Scripts") | %{
	Scaffold $ViewScaffolder -Controller $Controller -ViewName $_ -ModelType $ModelType -Template $_ -Area $Area -Layout $Layout -SectionNames $SectionNames -PrimarySectionName $PrimarySectionName -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders -Force:$Force
}

#if ($ModelType) {
#	$foundModelType = Get-ProjectType $ModelType -Project $Project
#	if (!$foundModelType) { return }
#	$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $Project)
#}


#if ($foundModelType) { $relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project) }
#if (!$relatedEntities) { $relatedEntities = @() }

#if($Related) {
#	foreach($relatedEntitie in $relatedEntities) {		
#		$relatedEntitieType = $relatedEntitie.RelationProperty.Type.AsString
#		if ($relatedEntitieType.StartsWith("System.Collections.Generic.ICollection<")) {
#			$relatedViewName = $relatedEntitie.RelatedEntityType.Name
#			$relatedModelType = $relatedEntitie.RelatedEntityType.FullName
#			Scaffold RelatedViews $Controller $relatedViewName -ModelType:$relatedModelType -Force:$Force
#			#Write-Host $relatedViewName + " - " + $relatedModelType
#		}
#	}
#}