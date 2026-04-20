using UnityEngine;

public abstract class UnitStatusEffectCreator : MonoBehaviour{
    [SerializeField] protected string statusName;

    public abstract UnitStatusEffect CreateStatus();
}