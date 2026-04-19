public abstract class UnitStatusEffect {
    protected CombatUnit target;

    public readonly string name;

    protected UnitStatusEffect(string name){
        this.name = name;
    }
    
    public virtual void OnApplied(CombatUnit targetTmp) {
        this.target = targetTmp;
    }

    public virtual void OnRemoved(){
        
    }
    
    public virtual bool CanExecuteAction(UnitAction action) {
        return true;
    }
}