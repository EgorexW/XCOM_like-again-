using System.Collections.Generic;
using UnityEngine;

public class SpawnAction : TargetedUnitAction{
    [SerializeField] GameObject prefabToSpawn;
    [Header("Spawn Settings")] [SerializeField] InsertTurnTakerType insertTurnTakerType = InsertTurnTakerType.Next;

    protected override void OnExecute(){
        targetNode.Spawn(prefabToSpawn, insertTurnTakerType);
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        var combatObject = prefabToSpawn.GetComponent<CombatObject>();
        if (!node.CanAcceptObject(combatObject)){
            result = TargetValidation.InvalidTarget;
        }
        if (!unit.GetCenterNode().LineUnobstructed(node, combatObject.GetBlockingFlags())){
            result |= TargetValidation.NoPath;
        }
        return result;
    }
}