using UnityEngine;

public class EliminatePayoutObjective : PayoutObjective{
    [SerializeField] int payoutPerEliminatedHostile = 50;

    [SerializeField] TeamFlag targetTeamFlag = TeamFlag.Enemy;
    [SerializeField] bool signPositive = true;

    int eliminatedCount = 0;
    public int PayoutSign => signPositive ? 1 : -1;

    public override void Init(CombatSystem CombatSystem){
        base.Init(CombatSystem);
        
        CombatSystem.onCombatObjectRemoved.AddListener(OnCombatObjectRemoved);
    }

    void OnCombatObjectRemoved(ICombatObject combatObject){
        
            var team = CombatSystem.TeamsSystem.GetTeam(combatObject);
            if (team == null){
                return;
            }
            if (team.Flags.HasFlag(targetTeamFlag)){
                eliminatedCount++;
            }
    }

    public override void UpdateObjective(CombatSystem combatSystem){
        SetPayout(eliminatedCount * payoutPerEliminatedHostile * PayoutSign);
    }
}