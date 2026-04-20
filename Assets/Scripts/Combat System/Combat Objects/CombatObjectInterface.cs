using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICombatObject{
    List<CombatGridNode> Nodes{ get; set; }
    CombatSystem CombatSystem { get; set; }
    CombatObjectFlags Flags { get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; }
    void MoveTo(List<CombatGridNode> targetNodes);
    public void Remove();
    void Init();
    UnityEvent<ICombatObject> onRemove{ get; }
    UnityEvent<ICombatObject> onInit{ get; }
}

[Flags]
public enum CombatObjectFlags{
    None = 0,
    Unit = 1 << 0,
    Wall = 1 << 1
}


public abstract class CombatComponent : MonoBehaviour{
    public ICombatObject CombatObject { get; set; }

    public virtual void Init(){ }
}