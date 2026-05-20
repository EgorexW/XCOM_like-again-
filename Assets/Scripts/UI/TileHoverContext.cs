using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Detects which CombatGridNode the mouse is currently hovering over and
/// broadcasts that context to any interested UI.  Nothing else needs to
/// raycast the grid — subscribe to the events here instead.
/// </summary>
public class TileHoverContext : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatGrid combatGrid;
    
    [SerializeField] float delay = 1f;
    
    [FoldoutGroup("Events")] public UnityEvent<CombatGridNode> onHoverTile = new();
    [FoldoutGroup("Events")] public UnityEvent                 onHoverClear = new();
    
    CombatGridNode _pendingNode;
    CombatGridNode _firedNode;
    float          _hoverTimer;
    
    protected void Update(){
        var worldPos = General.GetMouseWorldPos();
        var node     = combatGrid.GetNode(worldPos);
        if (node != _pendingNode){
            _pendingNode = node;
            _hoverTimer  = 0f;
            if (_firedNode != null){
                _firedNode = null;
                onHoverClear.Invoke();
            }
        }
        if (_pendingNode == null || _pendingNode == _firedNode) return;
        _hoverTimer += Time.deltaTime;
        if (_hoverTimer >= delay){
            _firedNode = _pendingNode;
            onHoverTile.Invoke(_firedNode);
        }
    }
}