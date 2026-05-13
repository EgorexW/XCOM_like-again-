using UnityEngine;

public class SpawnAction : TargetedUnitAction{
    [SerializeField] GameObject prefabToSpawn;

    protected override void OnExecute(){
        targetNode.Spawn(prefabToSpawn);
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