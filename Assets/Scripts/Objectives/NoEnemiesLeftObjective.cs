
    public class NoEnemiesLeftObjective : BattleObjective{
        public override void UpdateObjective(CombatSystem combatSystem){
            var teamsSystem = combatSystem.TeamsSystem;
            var teams = teamsSystem.Teams;
            IsCompleted = true;
            foreach (var team in teams){
                if (team.Empty){
                    continue;
                }
                foreach (var enemy in teamsSystem.GetEnemies(team)){
                    if (!enemy.Flags.HasFlag(CombatObjectFlags.Pacified)){
                        continue;
                    }
                    IsCompleted = false;
                    return;
                }
            }
        }
    }