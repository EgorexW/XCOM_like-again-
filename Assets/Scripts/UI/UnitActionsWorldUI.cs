using Sirenix.OdinInspector;
using UnityEngine;

public class UnitActionsWorldUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    void Awake(){
        combatSystem.onCombatObjectAdded.AddListener(OnCombatObjectAdded);
        combatSystem.onCombatObjectRemoved.AddListener(OnCombatObjectRemoved);
        objectsPool.onCreateObject.AddListener(OnCreateObject);
    }

    void OnCombatObjectRemoved(ICombatObject arg0){
        if (arg0 is Unit combatUnit){
            combatUnit.onActionPerformed.RemoveListener(OnActionPerformed);
        }
    }

    void OnCreateObject(GameObject arg0){
        var actionUI = arg0.GetComponent<UnitActionWorldUI>();
        actionUI.onHide.AddListener(element => objectsPool.RemoveObject(element.gameObject));
    }

    void OnCombatObjectAdded(ICombatObject arg0){
        if (arg0 is Unit combatUnit){
            combatUnit.onActionPerformed.AddListener(OnActionPerformed);
        }
    }

    void OnActionPerformed(UnitAction arg0){
        var obj = objectsPool.AddObject();
        obj.GetComponent<UnitActionWorldUI>().ShowAction(arg0);
        foreach (var activeObj in objectsPool.GetActiveObjs()){
            if (activeObj == obj){
                continue;
            }
            var distance = Vector3.Distance(activeObj.transform.position, obj.transform.position);
            activeObj.GetComponent<UnitActionWorldUI>().MakeSpace(distance);
        }
    }
}