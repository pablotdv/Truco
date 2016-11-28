select r.Rodada, j.CompeticaoFaseGrupoRodadaJogoId, s.[Set], 
	j.CompeticaoFaseGrupoEquipeUmId, s.EquipeUmTentos, s.EquipeDoisTentos, j.CompeticaoFaseGrupoEquipeDoisId,
	iif(EquipeUmTentos=24, j.CompeticaoFaseGrupoEquipeUmId, j.CompeticaoFaseGrupoEquipeDoisId)
from 
	CompeticoesFasesGruposRodadasJogosSets s, 
	CompeticoesFasesGruposRodadasJogos j, 
	CompeticoesFasesGruposRodadas r
where s.CompeticaoFaseGrupoRodadaJogoId = j.CompeticaoFaseGrupoRodadaJogoId
	and j.CompeticaoFaseGrupoRodadaId = r.CompeticaoFaseGrupoRodadaId
order by r.Rodada, j.CompeticaoFaseGrupoRodadaJogoId, s.[Set]