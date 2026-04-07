using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatObjectsUI : UIElement
{
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    public override void Show(){
        base.Show();
        UpdateUI();
    }

    void UpdateUI(){
        var combatObjects = combatSystem.CombatObjects;
        objectsPool.SetCount(combatObjects.Count);
        for (int i = 0; i < combatObjects.Count; i++){
            var combatObjectUI = objectsPool.GetActiveObject(i).GetComponent<CombatObjectUI>();
            combatObjectUI.SetCombatObject(combatObjects[i]);
        }
    }

    protected void Update(){
        if (IsVisible){
            UpdateUI();
        }
    }
}