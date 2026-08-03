using UnityEngine;

public class UnauthorizedForcePayoutObjective : PayoutObjective {
    [SerializeField] private int penaltyPerUnauthorizedKill = 200;
    [SerializeField] private TeamFlag targetTeamFlag = TeamFlag.Enemy;

    private int unauthorizedKillsCount;

    public override void Init(CombatSystem combatSystem) {
        base.Init(combatSystem);
        
        unauthorizedKillsCount = 0;
        
        foreach (var team in combatSystem.TeamsSystem.Teams) {
            if (team.Flags.HasFlag(targetTeamFlag)) {
                foreach (var member in team.CombatObjects) {
                    member.onRemove.AddListener(OnCombatObjectRemoved);
                }
            }
        }
    }

    private void OnCombatObjectRemoved(ICombatObject combatObject) {
        var suspectComponent = combatObject.GetCombatComponent<SuspectComponent>();
        if (suspectComponent != null) {
            if (suspectComponent.SuspectState != SuspectState.Hostile) {
                var healthComp = combatObject.GetCombatComponent<HealthComponent>();
                if (healthComp != null && healthComp.LastDamage.source != null) {
                    var killerTeam = combatObject.CombatSystem.TeamsSystem.GetTeam(healthComp.LastDamage.source);
                    if (killerTeam.Flags.HasFlag(TeamFlag.Player)) {
                        unauthorizedKillsCount++;
                    }
                }
            }
        }
    }

    public override void UpdateObjective(CombatSystem combatSystem){
        SetPayout(unauthorizedKillsCount * -penaltyPerUnauthorizedKill);
    }
}
