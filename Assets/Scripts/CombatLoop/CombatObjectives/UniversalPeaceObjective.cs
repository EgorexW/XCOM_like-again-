public class UniversalPeaceObjective : BattleObjective{
    public override void UpdateObjective(CombatSystem combatSystem){
        var teams = combatSystem.TeamsSystem.Teams;

        IsCompleted = true;

        foreach (var teamA in teams){
            // If team A is dead, they aren't fighting anyone. Skip them.
            if (!HasActiveMembers(teamA)){
                continue;
            }

            // Get team A's enemies
            var enemies = combatSystem.TeamsSystem.GetEnemyTeams(teamA);

            foreach (var enemyTeam in enemies)
                // If Team A is alive, AND their enemy is alive, the battle rages on!
                if (HasActiveMembers(enemyTeam)){
                    IsCompleted = false;
                    return;
                }
        }
    }

    bool HasActiveMembers(Team team){
        if (team.Empty){
            return false;
        }

        foreach (var member in team.CombatObjects)
            if (!member.Flags.HasFlag(CombatObjectFlags.Pacified)){
                return true;
            }
        return false;
    }
}