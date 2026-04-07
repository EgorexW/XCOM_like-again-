using UnityEngine;

public class SpawnAction : TargetedUnitAction{
    [SerializeField] GameObject prefabToSpawn;
    
    protected override void OnExecute(){
        var spawnedObj = Instantiate(prefabToSpawn, unit.transform.parent);
        var combatObj = spawnedObj.GetComponent<CombatObject>();
        if (combatObj == null){
            Debug.LogWarning($"Spawned object {spawnedObj.name} does not have a CombatObject component.", prefabToSpawn);
            return;
        }
        unit.Grid().PlaceCombatObject(combatObj, targetNode);
    }
}