using UnityEngine;

public class SuspectComponent : CombatComponent{
    [SerializeField] SuspectState suspectState = SuspectState.Docile;

    public SuspectState SuspectState => suspectState;

    public override void Init(){
        base.Init();
        if (CombatObject is Unit combatUnit){
            combatUnit.onActionPerformed.AddListener(OnActionPerformed);
        }
    }

    void OnActionPerformed(UnitAction action){
        if (action.ActionInfo.ActionFlags.HasFlag(ActionFlags.Aggressive)){
            ChangeState(SuspectState.Hostile);
        }
    }

    public void ChangeState(SuspectState newState){
        if (suspectState == newState){
            return;
        }
        suspectState = newState;
    }

    void OnDestroy(){
        if (CombatObject is Unit combatUnit){
            combatUnit.onActionPerformed.RemoveListener(OnActionPerformed);
        }
    }
}

public enum SuspectState{
    Docile, // Hands in pockets, walking around, running away. (Shooting = Penalty)
    Hostile, // Weapon drawn or actively attacking. (Shooting = Legal)
    Surrendered // Hands up, dropped weapon. (Shooting = Massive Penalty)
}