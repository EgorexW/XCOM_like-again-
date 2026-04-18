using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICombatObject{
    List<CombatGridNode> Nodes{ get; set; }
    CombatSystem CombatSystem { get; set; }
    GridOccupancyType OccupancyType { get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; }
    void MoveTo(List<CombatGridNode> targetNodes);
    public void Remove();
    void Init();
    UnityEvent<ICombatObject> onRemove{ get; }
    UnityEvent<ICombatObject> onInit{ get; }
}