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
    }

    void OnRemove(ICombatObject arg0){
        member.alive = false;
    }
}
