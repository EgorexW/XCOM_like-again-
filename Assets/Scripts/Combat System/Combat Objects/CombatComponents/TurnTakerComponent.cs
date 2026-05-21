using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public sealed class TurnTakerComponent : CombatComponent, ITurnTaker{
    [SerializeField] InsertTurnTakerType insertType = InsertTurnTakerType.Last;

    public InsertTurnTakerType InsertType { get => insertType; set => insertType = value; }

    public UnityAction<ITurnTaker> OnTurnCompleted{ get; set; }

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onStartTurn{ get; } = new();

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onEndTurn{ get; } = new();

    public override void Init(){
        base.Init();
        if (CombatObject is Unit){
            Debug.LogWarning($"TurnTakerComponent added to Unit '{CombatObject.Name}' but is not needed since Units are already turn takers.");
        }
        CombatObject.CombatSystem.TurnSystem.AddTurnTaker(this, insertType);
        CombatObject.onRemove.AddListener(OnCombatObjectRemoved);
    }

    void OnCombatObjectRemoved(ICombatObject arg0){
        TurnSystem.RemoveTurnTaker(this);
    }

    public void EndTurn(){
        onEndTurn.Invoke(this);
    }

    public void StartTurn(){
        onStartTurn.Invoke(this);
        
        CompleteTurn();
    }

    public TurnSystem TurnSystem{ get; set; }

    public void CompleteTurn(){
        OnTurnCompleted?.Invoke(this);
    }
}