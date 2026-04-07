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
        combatObject.MoveTo(combatGrid.GetNode(combatObject.WorldPosition));
        combatObject.onRemove.AddListener(RemoveCombatObject);
    }

    void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
        combatGrid.RemoveCombatObject(arg0);
    }

    public void StartCombat(){
        foreach (ICombatObject combatObject in combatObjects){
            combatObject.onInit.Invoke();
        }
        turnSystem.NextTurn();
        onCombatStarted.Invoke();
    }
}
