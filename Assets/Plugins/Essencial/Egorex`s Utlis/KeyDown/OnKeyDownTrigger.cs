using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

[RequireComponent(typeof(OnKeyDown))]
public abstract class OnKeyDownTrigger : MonoBehaviour
{
    [GetComponent] [SerializeField] OnKeyDown onKeyDown;

    void Awake()
    {
        onKeyDown.onKeyDown.AddListener(Trigger);
    }

    protected abstract void Trigger();
}