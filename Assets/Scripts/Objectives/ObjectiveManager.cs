using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour {
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    
    [Required][SerializeField] BattleObjective objective;

    void Awake() {
        combatSystem.onCombatStarted.AddListener(OnCombatStarted);
        combatSystem.onStateChanged.AddListener(CheckBattleState);
    }

    void OnCombatStarted(){
        objective.Init(combatSystem);
    }

    void CheckBattleState() {
        objective.UpdateObjective(combatSystem);
        if (objective.IsCompleted){
            EndBattle();
        }
    }

    void TriggerVictory() {
        Debug.Log("Battle Won!");
        EndBattle();
    }

    void EndBattle(){
        combatSystem.EndCombat();
    }

    void TriggerDefeat() {
        Debug.Log("Battle Lost!");
        EndBattle();
    }
}

public abstract class BattleObjective : MonoBehaviour{
    public bool IsCompleted { get; protected set; }
    
    public virtual void Init(CombatSystem combatSystem){
        
    }

    public abstract void UpdateObjective(CombatSystem combatSystem);
}