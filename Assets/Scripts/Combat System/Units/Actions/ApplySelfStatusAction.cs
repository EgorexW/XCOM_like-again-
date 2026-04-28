using System.Collections.Generic;
using UnityEngine;

public class ApplySelfStatusAction : UnitAction{
    [SerializeField] List<UnitModifierFactory> statusEffectCreators;

    protected override void OnExecute(){
        foreach (var creator in statusEffectCreators) unit.ApplyStatus(creator.CreateStatus());
    }
}