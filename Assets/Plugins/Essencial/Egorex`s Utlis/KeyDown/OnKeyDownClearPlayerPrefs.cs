using UnityEngine;

public class OnKeyDownClearPlayerPrefs : OnKeyDownTrigger{
    protected override void Trigger(){
        Debug.Log("Clearing all PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }
}