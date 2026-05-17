using UnityEngine;

public abstract class UnitModifierFactory : ScriptableObject{
    [SerializeField] protected string statusName;

    public abstract UnitModifier Create();

    protected void OnValidate(){
        if (string.IsNullOrEmpty(statusName)){
            statusName = name;
        }
    }
}