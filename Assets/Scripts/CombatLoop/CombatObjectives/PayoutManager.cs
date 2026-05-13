using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class PayoutManager : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;

    List<PayoutObjective> objectives = new();
    public IReadOnlyList<PayoutObjective> Objectives => objectives.AsReadOnly();

    void Awake(){
        combatSystem.onCombatStarted.AddListener(OnCombatStarted);
        combatSystem.onStateChanged.AddListener(UpdateObjectives);
        objectives = GetComponentsInChildren<PayoutObjective>().ToList();
    }

    void OnCombatStarted(){
        foreach (var objective in objectives) objective.Init(combatSystem);
    }

    void UpdateObjectives(){
        foreach (var objective in objectives) objective.UpdateObjective(combatSystem);
    }

    public int GetPayout(){
        UpdateObjectives();
        var payout = 0;
        foreach (var objective in objectives){
            Debug.Log($"Objective: {objective.name}, Payout: {objective.Payout}");
            payout += objective.Payout;
        }
        Debug.Log("Total Payout: " + payout);
        return payout;
    }
}

public abstract class PayoutObjective : MonoBehaviour{
    [SerializeField] string description;

    public string Description => description;

    [FoldoutGroup("Events")] public UnityEvent<int> onPayoutChanged = new();

    public int Payout{ get; private set; }

    public virtual void Init(CombatSystem combatSystem){ }

    public abstract void UpdateObjective(CombatSystem combatSystem);

    protected void SetPayout(int newPayout){
        Payout = newPayout;
        onPayoutChanged.Invoke(Payout);
    }
}