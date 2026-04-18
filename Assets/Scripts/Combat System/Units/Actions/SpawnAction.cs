using System.Collections.Generic;
using UnityEngine;

public class SpawnAction : TargetedUnitAction{
    [SerializeField] GameObject prefabToSpawn;
    [Header("Spawn Settings")]
    [SerializeField] InsertTurnTakerType insertTurnTakerType = InsertTurnTakerType.Next;

    public GameObject PrefabToSpawn => prefabToSpawn;

    protected override void OnExecute(){
        var spawnedObj = Instantiate(prefabToSpawn, unit.transform.parent);
        var combatObj = spawnedObj.GetComponent<CombatObject>();
        if (combatObj == null){
            Debug.LogWarning($"Spawned object {spawnedObj.name} does not have a CombatObject component.", prefabToSpawn);
            return;
        }
        Debug.Log($"Spawning object {spawnedObj.name} at node {targetNode}", spawnedObj);
        unit.CombatSystem.AddCombatObject(combatObj, new List<CombatGridNode>{ targetNode });
        var turnTaker = spawnedObj.GetComponentInChildren<ITurnTaker>();
        if (turnTaker != null){
            unit.CombatSystem.TurnSystem.AddTurnTaker(turnTaker, insertTurnTakerType);
        }
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        var combatObject = prefabToSpawn.GetComponent<CombatObject>();
        if (!node.CanAcceptObject(combatObject.OccupancyType)){
            result = TargetValidation.InvalidTarget;
        }
        if (!unit.GetCenterNode().LineUnobstructed(node, combatObject.OccupancyType)){
            result |= TargetValidation.NoPath;
        }
        return result;
    }
}