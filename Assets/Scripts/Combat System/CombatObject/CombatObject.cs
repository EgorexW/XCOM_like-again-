using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatObject : MonoBehaviour, ICombatObject{
    public CombatGridNode Node{ get; set; }
    
    public GameObject GameObject => gameObject;
    public bool OccupiesTile => true;
    public CombatGrid Grid => Node.grid;
    
    public void MoveTo(CombatGridNode targetPos){
        Node.grid.PlaceCombatObject(this, targetPos.GetPos());
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }

}

public interface ICombatObject{
    CombatGridNode Node { get; set; }
    GameObject GameObject{ get; }
    bool OccupiesTile { get; }
}