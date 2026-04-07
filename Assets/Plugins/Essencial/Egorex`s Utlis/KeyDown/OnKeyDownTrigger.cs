using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

[RequireComponent(typeof(OnKeyDown))]
public abstract class OnKeyDownTrigger : MonoBehaviour
{
    [GetComponent] [SerializeField] OnKeyDown onKeyDown;

    protected void Awake()
    {
        onKeyDown.onKeyDown.AddListener(Trigger);
    }

    protected abstract void Trigger();
}