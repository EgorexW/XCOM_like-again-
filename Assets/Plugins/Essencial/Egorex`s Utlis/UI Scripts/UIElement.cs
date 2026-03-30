using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIElement : MonoBehaviour
{
    public bool IsVisible => gameObject.activeSelf;
    
    [FoldoutGroup("Events")] public UnityEvent onShow;

    [FoldoutGroup("Events")] public UnityEvent onHide;

    public virtual void Show()
    {
        gameObject.SetActive(true);
        onShow.Invoke();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        onHide.Invoke();
    }
}