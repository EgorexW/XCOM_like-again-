using Sirenix.OdinInspector;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

public abstract class CountUI : UIElement
{
    [FoldoutGroup("Events")] public UnityEvent<float> onUpdate;

    public virtual void SetCount(int count)
    {
        Show();
        onUpdate.Invoke(count);
    }
}