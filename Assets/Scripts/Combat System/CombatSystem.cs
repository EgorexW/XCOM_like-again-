using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatSystem : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] CombatGrid combatGrid;
    [BoxGroup("References")][Required][SerializeField] public TurnSystem turnSystem;
    
    List<ICombatObject> combatObjects = new List<ICombatObject>();

    [FoldoutGroup("Events")]
    public UnityEvent onCombatStarted;

    public List<ICombatObject> CombatObjects => combatObjects.Copy();
    public CombatGrid CombatGrid => combatGrid;

    public void AddCombatObject(ICombatObject combatObject){
        combatObjects.Add(combatObject);
        combatGrid.PlaceCombatObject(combatObject, General.RoundVector(combatObject.GameObject.transform.position));
        combatObject.GameObject.transform.position = new Vector3(combatObject.Node.x, combatObject.Node.y, combatObject.GameObject.transform.position.z);
        combatObject.onRemove.AddListener(RemoveCombatObject);
    }

    void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
        combatGrid.RemoveCombatObject(arg0);
    }

    public void StartCombat(){
        turnSystem.NextTurn();
        onCombatStarted.Invoke();
    }
}
