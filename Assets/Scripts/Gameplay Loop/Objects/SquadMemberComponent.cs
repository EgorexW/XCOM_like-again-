using UnityEngine;

public class SquadMemberComponent : CombatComponent{
    SquadMember member;

    public void SetMember(SquadMember member){
        this.member = member;
    }

    public override void Init(){
        base.Init();
        CombatObject.Name = member.Name;
        CombatObject.onRemove.AddListener(OnRemove);
        var unit = CombatObject as Unit;
        if (unit == null){
            Debug.LogWarning("Object " + member.Name + " is not a combat unit");
            return;
        }
        foreach (var equipment in member.Equipment){
            var modifier = equipment.GetModifier();
            modifier.onRemoved.AddListener(_ => RemoveEquipment(equipment));
            unit.ApplyModifier(modifier);
        }
    }

    void RemoveEquipment(Equipment equipment){
        member.RemoveEquipment(equipment);
    }

    void OnRemove(ICombatObject arg0){
        member.alive = false;
    }
}