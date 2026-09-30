using System;
using UnityEngine;

[Serializable]
public class ProgressionState{
    [SerializeField] int step = 0;
    
    public int Step => step;
    
    public event Action onChanged;

    public void IncrementStep(){
        // TODO Win Game
        step++;
        Debug.Log($"Progression step incremented to {step}");
        onChanged?.Invoke();
    }
}