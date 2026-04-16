
    using System.Collections.Generic;
    using UnityEngine;

    public class SurrenderAction : UnitAction{
        protected override void OnExecute(){
            var suspectComponent = unit.GetCombatComponent<SuspectComponent>();
            if (suspectComponent != null){
                suspectComponent.ChangeState(SuspectState.Surrendered);
            }
            else{
                Debug.LogWarning("SuspectComponent is null", this);
            }
            unit.ApplyStatus(CreateSurrenderedStatus());
        }

        UnitStatusEffect CreateSurrenderedStatus(){
            return new SurrenderedStatus();
        }
    }

    class SurrenderedStatus : UnitStatusEffect{
        List<UnitAction> allowedActions;

        public SurrenderedStatus(List<UnitAction> allowedActions) {
            this.allowedActions = allowedActions;
        }

        public SurrenderedStatus(){
            this.allowedActions = new List<UnitAction>();
        }

        public override bool CanExecuteAction(UnitAction action){
            return allowedActions.Contains(action);
        }
    }