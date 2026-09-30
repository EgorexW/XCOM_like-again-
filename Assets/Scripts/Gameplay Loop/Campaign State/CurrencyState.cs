using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class CurrencyState{
    [FormerlySerializedAs("money")] [SerializeField] int value;
    
    public int Value => value;
    
    public event Action onChanged;
    
    public void SetValue(int amount){
        value = amount;
        onChanged?.Invoke();
    }

    public void ChangeValue(int amount){
        value += amount;
        onChanged?.Invoke();
    }
}