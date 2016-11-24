[T4Scaffolding.Scaffolder(Description = "Enter a description of All here")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Scaffold Portugues.ControllersWithViews Atletas -ModelType:Truco.Models.Atleta -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Bairros -ModelType:Truco.Models.Bairro -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Cidades -ModelType:Truco.Models.Cidade -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Competicoes -ModelType:Truco.Models.Competicao -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews CompeticoesEquipes -ModelType:Truco.Models.CompeticaoEquipe -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Enderecos -ModelType:Truco.Models.Endereco -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Entidades -ModelType:Truco.Models.Entidade -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Equipes -ModelType:Truco.Models.Equipe -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews EquipesAtletas -ModelType:Truco.Models.EquipeAtleta -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Estados -ModelType:Truco.Models.Estado -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Logradouros -ModelType:Truco.Models.Logradouro -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Paises -ModelType:Truco.Models.Pais -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Regioes -ModelType:Truco.Models.Regiao -DbContextType:Truco.Model.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews RegioesCidades -ModelType:Truco.Models.RegiaoCidade -DbContextType:Truco.Model.TrucoDbContext -Force:$Force