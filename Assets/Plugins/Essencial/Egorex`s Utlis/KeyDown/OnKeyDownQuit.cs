using UnityEngine;

public class OnKeyDownQuit : OnKeyDownTrigger
{
    protected override void Trigger()
    {
#if !UNITY_WEBGL && !UNITY_EDITOR
        Application.Quit();
#else
        Debug.LogWarning("Application.Quit() is not supported in WebGL or Editor mode.");
#endif
    }
}