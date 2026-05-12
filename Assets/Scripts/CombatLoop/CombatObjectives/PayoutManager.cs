using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PayoutManager : MonoBehaviour {
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    
    [Required][SerializeField] List<PlayoutObjective> objectives;

    void Awake() {
        combatSystem.onCombatStarted.AddListener(OnCombatStarted);
        combatSystem.onStateChanged.AddListener(UpdateObjectives);
    }

    void OnCombatStarted(){
        foreach (PlayoutObjective objective in objectives){
            objective.Init(combatSystem);
        }
    }

    void UpdateObjectives() {
        foreach (PlayoutObjective objective in objectives){
            objective.UpdateObjective(combatSystem);
        }
    }

    public int GetPayout(){
        UpdateObjectives();
        int payout = 0;
        foreach (PlayoutObjective objective in objectives){
            payout += objective.Playout;
        }
        return payout;
    }
}

public abstract class PlayoutObjective : MonoBehaviour{
    public int Playout { get; protected set; }
    
    public virtual void Init(CombatSystem combatSystem){
        
    }

    public abstract void UpdateObjective(CombatSystem combatSystem);
}