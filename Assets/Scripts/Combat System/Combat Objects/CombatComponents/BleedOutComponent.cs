using Sirenix.OdinInspector;
using UnityEngine;

public class BleedOutComponent : TurnTakerComponent{
    [SerializeField] float killChance = 0.5f;
    [SerializeField] Vector2Int bleedOutTurnsRange = new Vector2Int(2, 4);
    [SerializeField] UnitModifierFactory statusToApply;

    public bool IsBleedingOut { get; private set; }
    public int TurnsLeft { get; private set; }

    public void BleedOut(){
        if (IsBleedingOut || Random.value < killChance){
            Die();
            return;
        }
        
        IsBleedingOut = true;
        TurnsLeft = bleedOutTurnsRange.Random();

        var unit = CombatObject as Unit;
        if (unit != null && statusToApply != null){
            unit.ApplyModifier(statusToApply.Create());
        }
    }

    public override void StartTurn(){
        base.StartTurn();
        if (IsBleedingOut){
            TurnsLeft -= 1;
            if (TurnsLeft <= 0){
                Die();
            }
            else{
                CompleteTurn();
            }
        }
        else{
            CompleteTurn();
        }
    }

    void Die(){
        CombatObject.Remove();
    }
}
