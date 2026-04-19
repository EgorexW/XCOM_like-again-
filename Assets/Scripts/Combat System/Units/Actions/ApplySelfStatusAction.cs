
    using System.Collections.Generic;
    using UnityEngine;

    public class ApplySelfStatusAction : UnitAction{
        [SerializeField] List<UnitStatusEffectCreator> statusEffectCreators;
        
        protected override void OnExecute(){
            foreach (UnitStatusEffectCreator creator in statusEffectCreators){
                unit.ApplyStatus(creator.CreateStatus());
            }
        }
    }