with v1 as (
select r.Rodada, j.CompeticaoFaseGrupoRodadaJogoId, s.[Set], 
	j.CompeticaoFaseGrupoEquipeUmId, s.EquipeUmTentos, s.EquipeDoisTentos, j.CompeticaoFaseGrupoEquipeDoisId,
	iif(EquipeUmTentos=24, j.CompeticaoFaseGrupoEquipeUmId, j.CompeticaoFaseGrupoEquipeDoisId) as Ganhador
from 
	CompeticoesFasesGruposRodadasJogosSets s, 
	CompeticoesFasesGruposRodadasJogos j, 
	CompeticoesFasesGruposRodadas r
where s.CompeticaoFaseGrupoRodadaJogoId = j.CompeticaoFaseGrupoRodadaJogoId
	and j.CompeticaoFaseGrupoRodadaId = r.CompeticaoFaseGrupoRodadaId
)
, v2 as (
select v1.Rodada, v1.CompeticaoFaseGrupoRodadaJogoId, sum(EquipeUmTentos) TentosUm, Sum(EquipeDoisTentos) TentosDois, 
	sum( case 
			 when v1.CompeticaoFaseGrupoEquipeUmId = v1.Ganhador  then 1
			 else 0
		 end) SetsUm,
	sum( case 
			 when v1.CompeticaoFaseGrupoEquipeDoisId = v1.Ganhador  then 1
			 else 0
		 end) SetsDois, v1.CompeticaoFaseGrupoEquipeUmId, v1.CompeticaoFaseGrupoEquipeDoisId
from v1
group by v1.Rodada, v1.CompeticaoFaseGrupoRodadaJogoId, v1.CompeticaoFaseGrupoEquipeUmId, v1.CompeticaoFaseGrupoEquipeDoisId)

select *,
	case 
		when v2.SetsUm >= 2 then v2.CompeticaoFaseGrupoEquipeUmId
		when v2.SetsDois >= 2 then v2.CompeticaoFaseGrupoEquipeDoisId
	end GanhadorId
from v2
order by v2.Rodada, v2.CompeticaoFaseGrupoRodadaJogoId
