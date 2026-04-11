using UnityEngine;

public class SpawnAction : TargetedUnitAction{
    [SerializeField] GameObject prefabToSpawn;
    [Header("Spawn Settings")]
    [SerializeField] InsertTurnTakerType insertTurnTakerType = InsertTurnTakerType.Next;

    protected override void OnExecute(){
        var spawnedObj = Instantiate(prefabToSpawn, unit.transform.parent);
        var combatObj = spawnedObj.GetComponent<CombatObject>();
        if (combatObj == null){
            Debug.LogWarning($"Spawned object {spawnedObj.name} does not have a CombatObject component.", prefabToSpawn);
            return;
        }
        unit.CombatSystem.AddCombatObject(combatObj);
        combatObj.MoveTo(targetNode);
        var turnTaker = spawnedObj.GetComponentInChildren<ITurnTaker>();
        if (turnTaker != null){
            unit.CombatSystem.TurnSystem.AddTurnTaker(turnTaker, insertTurnTakerType);
        }
    }

    protected override bool CheckActionSpecificRules(CombatGridNode node){
        var combatObject = prefabToSpawn.GetComponent<CombatObject>();
        if (!node.CanAcceptObject(combatObject.OccupancyType)){
            return false;
        }
        if (!unit.Node.LineUnobstructed(node, combatObject.OccupancyType)){
            return false;
        }
        return true;
    }
}