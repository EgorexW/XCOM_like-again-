using System.Collections.Generic;
using UnityEngine;

public abstract class Level : MonoBehaviour{
    public List<CombatObject> GetCombatObjects(){
        return new List<CombatObject>(GetComponentsInChildren<CombatObject>());
    }

    public abstract List<Vector2> GetSpawnPoints(int team);
}