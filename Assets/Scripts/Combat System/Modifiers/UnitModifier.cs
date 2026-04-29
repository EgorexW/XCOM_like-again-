using Sirenix.OdinInspector;
using UnityEngine.Events;

public abstract class UnitModifier{
    protected Unit target;

    public readonly string name;
    public readonly UnitModifierFactory sourceDefinition; 

    public UnityEvent<UnitModifier> onRemoved = new UnityEvent<UnitModifier>();

    protected UnitModifier(string name, UnitModifierFactory sourceDefinition){
        this.name = name;
        this.sourceDefinition = sourceDefinition;
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