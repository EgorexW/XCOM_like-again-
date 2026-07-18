using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class PayoutManager : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;

    List<PayoutObjective> objectives = new();
    public IReadOnlyList<PayoutObjective> Objectives => objectives.AsReadOnly();

    protected void Awake(){
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

    public void AddObjective(PayoutObjective objective){
        if (!objectives.Contains(objective))
            objectives.Add(objective);
    }

    public int GetPayout(){
        UpdateObjectives();
        var payout = 0;
        foreach (var objective in objectives){
            // Debug.Log($"Objective: {objective.name}, Payout: {objective.Payout}");
            payout += objective.Payout;
        }
        // Debug.Log("Total Payout: " + payout);
        return payout;
    }
}

public abstract class PayoutObjective : MonoBehaviour{
    [InfoBox("Empty description means the objective is hidden", InfoMessageType.Info, "IsHidden")]
    [SerializeField] string description;

    protected CombatSystem CombatSystem { get; private set; }
    
    public string Description => description;
    public bool IsHidden => description.IsNullOrWhitespace();

    [FoldoutGroup("Events")] public UnityEvent<int> onPayoutChanged = new();

    public int Payout{ get; private set; }

    public virtual void Init(CombatSystem combatSystem){
        this.CombatSystem = combatSystem;
    }

    public abstract void UpdateObjective(CombatSystem combatSystem);

    protected void SetPayout(int newPayout){
        Payout = newPayout;
        onPayoutChanged.Invoke(Payout);
    }
}