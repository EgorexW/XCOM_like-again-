using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class TurnTakerComponentBase : CombatComponent{
    protected bool active;
    
    public override void Init() {
        base.Init();
        active = true;
        ResolveTurnTaker();
    }

    protected virtual void ResolveTurnTaker() {
        if (CombatObject is Unit unit) {
            unit.onStartTurn.AddListener(OnUnitStartTurn);
            unit.onEndTurn.AddListener(OnUnitEndTurn);
        }
        else {
            var mainTurnTaker = CombatObject.GetCombatComponent<TurnTakerComponent>();
            if (mainTurnTaker == null) {
                Debug.LogWarning($"TurnTakerComponent was missing on non-Unit CombatObject '{CombatObject.Name}' but is required by '{GetType().Name}'.");
                return;
            }
            mainTurnTaker.onStartTurn.AddListener(OnTurnTakerStartTurn);
            mainTurnTaker.onEndTurn.AddListener(OnTurnTakerEndTurn);
        }
    }

    private void OnUnitStartTurn(Unit unit) {
        if (active){
            OnStartTurn();
        }
    }

    private void OnUnitEndTurn(Unit unit) {
        if (active){
            OnEndTurn();
        }
    }

    private void OnTurnTakerStartTurn(ITurnTaker tt) {
        if (active){
            OnStartTurn();
        }
    }

    private void OnTurnTakerEndTurn(ITurnTaker tt) {
        if (active){
            OnEndTurn();
        }
    }

    protected virtual void OnStartTurn() {}

    protected virtual void OnEndTurn() {}
}
