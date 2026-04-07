using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatGrid : MonoBehaviour{
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;

    public Grid<CombatGridNode> Grid{ get; private set; }

    [FoldoutGroup("Events")] public UnityEvent<CombatGridNode> onCombatGridNodeChanged = new();

    protected void Awake(){
        Grid = new Grid<CombatGridNode>(width, height, 1f, Vector3.zero,
            (g, x, y) => new CombatGridNode(this, x, y)
        );

        Grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }

    protected void OnDestroy(){
        if (Grid != null){
            Grid.OnGridObjectChanged -= Grid_OnGridObjectChanged;
        }
    }

    void Grid_OnGridObjectChanged(object sender, Grid<CombatGridNode>.OnGridObjectChangedEventArgs e){
        var node = Grid.GetGridObject(e.x, e.y);
        onCombatGridNodeChanged.Invoke(node);
    }

    public void PlaceCombatObject(ICombatObject combatObject, CombatGridNode newNode){
        if (combatObject.Node != null){
            combatObject.Node.RemoveCombatObject(combatObject);
        }
        newNode.AddCombatObject(combatObject);
        combatObject.Node = newNode;
    }

    public void TriggerGridObjectChanged(CombatGridNode node){
        // Debug.Log($"A grid object was changed at coordinates X: {node.x}, Y: {node.y}");
    }

    public CombatGridNode GetNode(Vector2 pos){
        return Grid.GetGridObject(pos);
    }

    public void RemoveCombatObject(ICombatObject arg0){
        if (arg0.Node != null){
            arg0.Node.RemoveCombatObject(arg0);
            arg0.Node = null;
        }
    }

}