using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PayoutObjectivesUI : UIElement {
    [BoxGroup("References")] [Required] [SerializeField] PayoutManager payoutManager;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    void Start(){
        Show();
    }

    public override void Show() {
        base.Show();
        var objectives = payoutManager.Objectives;
        objectsPool.SetCount(objectives.Count);
        for (var i = 0; i < objectives.Count; i++) {
            var objectiveUI = objectsPool.GetActiveObject(i).GetComponent<PayoutObjectiveUI>();
            objectiveUI.Show(objectives[i]);
        }
    }
}
