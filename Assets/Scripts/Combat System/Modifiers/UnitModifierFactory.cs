using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public abstract class UnitModifierFactory : ScriptableObject{
    [SerializeField] protected ModifierInfo modifierInfo;

    public abstract UnitModifier Create();
}