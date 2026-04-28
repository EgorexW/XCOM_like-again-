
    public class NoEnemiesLeftObjective : BattleObjective{
        public override void UpdateObjective(CombatSystem combatSystem){
            var teamsSystem = combatSystem.TeamsSystem;
            var teams = teamsSystem.Teams;
            IsCompleted = true;
            foreach (var team in teams){
                if (team.Empty){
                    continue;
                }
                if (teamsSystem.GetEnemies(team).Count <= 0){
                    continue;
                }
                IsCompleted = false;
                return;
            }
        }
    }