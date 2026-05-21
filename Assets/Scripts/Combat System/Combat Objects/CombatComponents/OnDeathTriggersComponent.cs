using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OnDeathTriggersComponent : CombatComponent{
    [SerializeField] List<CombatEffect> onDeathEffects;

    public override void Init(){
        base.Init();
        CombatObject.onRemove.AddListener(OnCombatObjectRemoved);
    }

    void OnDestroy(){
        CombatObject.onRemove.RemoveListener(OnCombatObjectRemoved);
    }

    void OnCombatObjectRemoved(ICombatObject combatObject){
        if (onDeathEffects == null) return;
        foreach (var effect in onDeathEffects){
            effect.targetNode = combatObject.GetCenterNode();
            effect.Execute();
        }
    }
}
