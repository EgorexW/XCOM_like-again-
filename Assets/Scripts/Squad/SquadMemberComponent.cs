using UnityEngine;

public class SquadMemberComponent : CombatComponent
{
    SquadMember member;

    public void SetMember(SquadMember member){
        this.member = member;
    }

    public override void Init(){
        base.Init();
        CombatObject.Name = member.name;
        CombatObject.onRemove.AddListener(OnRemove);
        var unit = CombatObject as Unit;
        if (unit == null){
            Debug.LogWarning("Object " + member.name + " is not a combat unit");
            return;
        }
        foreach (var modifierFactory in member.modifiers){
            var modifier = modifierFactory.Create();
            modifier.onRemoved.AddListener(OnModifierRemoved);
            unit.ApplyModifier(modifier);
        }
    }

    void OnModifierRemoved(UnitModifier arg0){
        member.modifiers.Remove(arg0.sourceDefinition);
    }

    void OnRemove(ICombatObject arg0){
        member.alive = false;
    }
}
