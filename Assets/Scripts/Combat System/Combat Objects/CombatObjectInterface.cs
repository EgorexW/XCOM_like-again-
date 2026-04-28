using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICombatObject{
    List<CombatGridNode> Nodes{ get; set; }
    CombatSystem CombatSystem{ get; set; }
    CombatObjectFlags Flags{ get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; set; }
    void MoveTo(List<CombatGridNode> targetNodes);
    public void Remove();
    void Init();
    UnityEvent<ICombatObject> onRemove{ get; }
    UnityEvent<ICombatObject> onInit{ get; }
}

[Flags]
public enum CombatObjectFlags{
    None = 0,
    Object = 1 << 0,
    Wall = 1 << 1,
    [InspectorName("LoS Blocker")] LoSBlocker = 1 << 2,
    MovementBlocker = 1 << 3,

    [InspectorName("Presets/Standard Wall")] StandardWall = Object | Wall | LoSBlocker | MovementBlocker,
    [InspectorName("Presets/Standard Object")] StandardObject = Object | LoSBlocker | MovementBlocker
}


public abstract class CombatComponent : MonoBehaviour{
    public ICombatObject CombatObject{ get; set; }

    public virtual void Init(){ }
}