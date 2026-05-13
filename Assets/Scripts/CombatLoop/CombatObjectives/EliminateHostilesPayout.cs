using UnityEngine;

public class EliminateHostilesPayoutObjective : PayoutObjective{
    [SerializeField] int payoutPerEliminatedHostile = 50;

    [SerializeField] TeamFlag targetTeamFlag = TeamFlag.Enemy;

    int initialHostileCount;

    public override void Init(CombatSystem combatSystem){
        base.Init(combatSystem);

        initialHostileCount = 0;
        foreach (var team in combatSystem.TeamsSystem.Teams)
            if (team.Flags.HasFlag(targetTeamFlag)){
                initialHostileCount += team.CombatObjects.Count;
            }
    }

    public override void UpdateObjective(CombatSystem combatSystem){
        var currentHostileCount = 0;
        foreach (var team in combatSystem.TeamsSystem.Teams)
            if (team.Flags.HasFlag(targetTeamFlag)){
                currentHostileCount += team.CombatObjects.Count;
            }

        var eliminatedCount = initialHostileCount - currentHostileCount;

        // Ensure we don't drop below zero in case of unexpected additions to enemy teams
        if (eliminatedCount < 0){
            eliminatedCount = 0;
        }

        SetPayout(eliminatedCount * payoutPerEliminatedHostile);
    }
}