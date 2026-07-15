using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatObjectsTooltip : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TileHoverContext tileHoverContext;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool pool;

    protected void Awake(){
        tileHoverContext.onHoverTile.AddListener(OnHoverTile);
        tileHoverContext.onHoverClear.AddListener(OnHoverClear);
    }

    void OnHoverClear(){
        Hide();
    }

    void OnHoverTile(CombatGridNode arg0){
        Show();
        var combatObjects = arg0.GetCombatObjects();
        var messages = new List<Message>();
        foreach (var combatObject in combatObjects){
            var message = combatObject.GetMessage();
            if (message.HasValue){
                messages.Add(message.Value);
            }
        }
        pool.SetCount(messages.Count);
        for (int i = 0; i < messages.Count; i++){
            var obj = pool.GetActiveObject(i);
            var message = messages[i];
            var messageUI = obj.GetComponent<MessageUI>();
            messageUI.Show(message);
        }
    }
}
