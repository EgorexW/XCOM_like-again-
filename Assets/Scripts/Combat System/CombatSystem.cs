using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] CombatGrid combatGrid;
    [BoxGroup("References")][Required][SerializeField] public TurnSystem turnSystem;
    
    List<ICombatObject> combatObjects = new List<ICombatObject>();
    
    public List<ICombatObject> CombatObjects => combatObjects.Copy();

    public void AddCombatObject(ICombatObject combatObject){
        combatObjects.Add(combatObject);
        combatGrid.PlaceCombatObject(combatObject, General.RoundVector(combatObject.GameObject.transform.position));
    }

    public void StartCombat(){
        turnSystem.NextTurn();
    }
}
