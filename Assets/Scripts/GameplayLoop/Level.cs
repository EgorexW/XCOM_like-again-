using System.Collections.Generic;
using UnityEngine;

public abstract class Level : MonoBehaviour{
    public List<CombatObject> GetCombatObjects(){
        return new List<CombatObject>(GetComponentsInChildren<CombatObject>());
    }

    public abstract List<ITurnTaker> GetTurnTakers();
    public abstract List<Team> GetTeams();
}