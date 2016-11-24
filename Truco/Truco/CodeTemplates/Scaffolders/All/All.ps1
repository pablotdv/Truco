[T4Scaffolding.Scaffolder(Description = "Enter a description of All here")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Scaffold Portugues.ControllersWithViews Atletas -ModelType:Truco.Models.Atleta -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Bairros -ModelType:Truco.Models.Bairro -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Cidades -ModelType:Truco.Models.Cidade -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Competicoes -ModelType:Truco.Models.Competicao -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews CompeticoesEquipes -ModelType:Truco.Models.CompeticaoEquipe -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Enderecos -ModelType:Truco.Models.Endereco -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Entidades -ModelType:Truco.Models.Entidade -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Equipes -ModelType:Truco.Models.Equipe -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews EquipesAtletas -ModelType:Truco.Models.EquipeAtleta -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Estados -ModelType:Truco.Models.Estado -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Logradouros -ModelType:Truco.Models.Logradouro -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Paises -ModelType:Truco.Models.Pais -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews Regioes -ModelType:Truco.Models.Regiao -DbContextType:Truco.Models.TrucoDbContext -Force:$Force
Scaffold Portugues.ControllersWithViews RegioesCidades -ModelType:Truco.Models.RegiaoCidade -DbContextType:Truco.Models.TrucoDbContext -Force:$Force