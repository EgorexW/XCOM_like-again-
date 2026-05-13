using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PayoutManager : MonoBehaviour {
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    
    List<PayoutObjective> objectives = new();

    void Awake() {
        combatSystem.onCombatStarted.AddListener(OnCombatStarted);
        combatSystem.onStateChanged.AddListener(UpdateObjectives);
        objectives = GetComponentsInChildren<PayoutObjective>().ToList();
    }

    void OnCombatStarted(){
        foreach (PayoutObjective objective in objectives){
            objective.Init(combatSystem);
        }
    }

    void UpdateObjectives() {
        foreach (PayoutObjective objective in objectives){
            objective.UpdateObjective(combatSystem);
        }
    }

    public int GetPayout(){
        UpdateObjectives();
        int payout = 0;
        foreach (PayoutObjective objective in objectives){
            Debug.Log($"Objective: {objective.name}, Payout: {objective.Payout}");
            payout += objective.Payout;
        }
        Debug.Log("Total Payout: " + payout);
        return payout;
    }
}

public abstract class PayoutObjective : MonoBehaviour{
    public int Payout { get; protected set; }
    
    public virtual void Init(CombatSystem combatSystem){
        
    }

    public abstract void UpdateObjective(CombatSystem combatSystem);
}