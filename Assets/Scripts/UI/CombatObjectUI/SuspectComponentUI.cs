using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SuspectComponentUI : CombatObjectUI{
    [BoxGroup("References")] [Required] [SerializeField] Image image;

    [InfoBox("$SuspectStateInfo")] [SerializeField] List<Color> colorPerSuspectState;

    public string SuspectStateInfo => General.EnumDescription<SuspectState>();

    public void Show(SuspectComponent suspectComponent){
        base.Show();
        var index = (int)suspectComponent.SuspectState;
        index = Mathf.Clamp(index, 0, colorPerSuspectState.Count);
        image.color = colorPerSuspectState[index];
    }

    public override void SetCombatObject(ICombatObject combatObject){
        var suspectComponent = combatObject.GetCombatComponent<SuspectComponent>();
        if (suspectComponent != null){
            Show(suspectComponent);
        }
        else{
            Hide();
        }
    }
}