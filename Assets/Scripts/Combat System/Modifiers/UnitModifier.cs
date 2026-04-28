public abstract class UnitModifier{
    protected CombatUnit target;

    public readonly string name;

    protected UnitModifier(string name){
        this.name = name;
    }

    public virtual void OnApplied(CombatUnit targetTmp){
        target = targetTmp;
    }

    public virtual void OnRemoved(){ }

    public virtual bool CanExecuteAction(UnitAction action){
        return true;
    }
}

public static class StringKeys
{
    public const string AssetMenuModifierBasePath = "Modifier/";
}