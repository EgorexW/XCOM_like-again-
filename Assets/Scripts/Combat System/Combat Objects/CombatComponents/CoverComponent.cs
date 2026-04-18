using UnityEditor;
using UnityEngine;

public class CoverComponent : CombatComponent{
    [SerializeField] Direction direction;
    [SerializeField] Transform coverSprite;

    [SerializeField] GameObject adjacentCoverPrefab;

    public Direction Direction => direction;

    public bool spawnedAsAdjacentCover = false;


    protected void Start(){
        UpdateVisuals();
    }

    public override void Init(){
        base.Init();
        SpawnAdjacentCover();
    }

    void SpawnAdjacentCover(){
        if (adjacentCoverPrefab == null || spawnedAsAdjacentCover){
            return;
        }

        var targetPos = combatObject.GetCenterNode().GetPos() + direction.Vector();
        var targetNode = combatObject.Grid().GetNode(targetPos);

        if (targetNode == null){
            return;
        }

        var spawnedHalf = Instantiate(adjacentCoverPrefab, transform.parent);
        
        var combatObj = spawnedHalf.GetComponent<CombatObject>();
        var coverComponent = spawnedHalf.GetComponentInChildren<CoverComponent>();
        coverComponent.spawnedAsAdjacentCover = true;
        
        combatObject.CombatSystem.AddCombatObject(combatObj, targetNode);
        coverComponent.InitializeAsSpawnedHalf(direction.Opposite(), targetNode);

    }

    public void InitializeAsSpawnedHalf(Direction oppositeDir, CombatGridNode targetNode){
        direction = oppositeDir;
        UpdateVisuals();
    }

    void UpdateVisuals(){
        if (coverSprite == null){
            return;
        }
        coverSprite.localRotation = Quaternion.Euler(0, 0, direction.Angle());
    }

#if UNITY_EDITOR
    protected void OnValidate(){
        EditorApplication.delayCall += EditorDelayVisualUpdate;
    }

    void EditorDelayVisualUpdate(){
        EditorApplication.delayCall -= EditorDelayVisualUpdate;
        if (this == null || coverSprite == null){
            return;
        }

        UpdateVisuals();
        EditorUtility.SetDirty(coverSprite);
    }
#endif
}