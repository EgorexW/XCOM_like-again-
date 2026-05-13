using UnityEngine;

public class PacifyHostilesPayoutObjective : PayoutObjective{
    [SerializeField] int payoutPerPacifiedHostile = 100;

    [SerializeField] TeamFlag targetTeamFlag = TeamFlag.Enemy;

    public override void UpdateObjective(CombatSystem combatSystem){
        var pacifiedCount = 0;
        foreach (var team in combatSystem.TeamsSystem.Teams)
            if (team.Flags.HasFlag(targetTeamFlag)){
                foreach (var member in team.CombatObjects)
                    if (member.Flags.HasFlag(CombatObjectFlags.Pacified)){
                        pacifiedCount++;
                    }
            }

        SetPayout(pacifiedCount * payoutPerPacifiedHostile);
    }
}