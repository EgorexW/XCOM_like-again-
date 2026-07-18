using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitModifier{
    protected Unit target;

    public ModifierInfo Info{ get; }

    public UnityEvent<UnitModifier> onRemoved = new();

    protected UnitModifier(ModifierInfo info){
        this.Info = info;
    }

    public virtual void OnApplied(Unit targetTmp){
        target = targetTmp;
    }

    public virtual void OnRemoved(){
        onRemoved.Invoke(this);
    }

    public virtual bool CanExecuteAction(UnitAction action){
        return true;
    }
}

[Serializable]
[HideLabel]
public class ModifierInfo{
    [SerializeField] string modifierName;
    [SerializeField] [TextArea] string description;
    [SerializeField] ModifierFlags modifierFlags;

    public string Name => modifierName;
    public string Description => description;
    public ModifierFlags ModifierFlags => modifierFlags;
}

[Flags]
public enum ModifierFlags{
    Buff = 1 << 0,
    Debuff = 1 << 1,
    Item = 1 << 2
}