using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SuspectComponentUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] Image image;

    [InfoBox("$SuspectStateInfo")]
    [SerializeField] List<Color> colorPerSuspectState;
    
    private string SuspectStateInfo {
        get {
            string info = "List Index / States:\n";
            foreach (SuspectState state in Enum.GetValues(typeof(SuspectState))) {
                info += $"{(int)state}: {state}\n"; 
            }
            return info.TrimEnd();
        }
    }
    
    public void Show(SuspectComponent suspectComponent){
        base.Show();
        var index = (int)suspectComponent.SuspectState;
        index = Mathf.Clamp(index, 0, colorPerSuspectState.Count);
        image.color = colorPerSuspectState[index];
    }
}