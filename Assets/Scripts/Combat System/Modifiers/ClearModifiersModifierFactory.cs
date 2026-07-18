using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Clear Modifiers Modifier")]
public class ClearModifiersModifierFactory : UnitModifierFactory{
    [SerializeField] ModifierFlags clearFlags;
    
    public override UnitModifier Create(){
        return new ClearModifiersModifier(modifierInfo, clearFlags);
    }
}

public class ClearModifiersModifier : UnitModifier{
    readonly ModifierFlags flags;

    public ClearModifiersModifier(ModifierInfo info, ModifierFlags flags) : base(info){
        this.flags = flags;
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        var statuses = targetTmp.ActiveStatuses.Copy();
        foreach (var status in statuses){
            if ((status.Info.ModifierFlags & flags) != 0){
                target.RemoveModifier(status);
            }
        }
        targetTmp.RemoveModifier(this);
    }
}
