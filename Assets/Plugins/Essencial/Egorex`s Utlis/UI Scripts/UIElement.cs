using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIElement : MonoBehaviour{
    public bool IsVisible => gameObject.activeSelf;

    [FoldoutGroup("Events")] public UnityEvent<UIElement> onShow;

    [FoldoutGroup("Events")] public UnityEvent<UIElement> onHide;

    public virtual void Show(){
        gameObject.SetActive(true);
        onShow.Invoke(this);
    }

    public virtual void Hide(){
        gameObject.SetActive(false);
        onHide.Invoke(this);
    }
}