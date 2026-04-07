using UnityEditor;
using UnityEngine;

public class CoverComponent : CombatComponent{
    [SerializeField] Direction direction;
    [SerializeField] Transform coverSprite;

    [SerializeField] GameObject adjacentCoverPrefab;

    public Direction Direction => direction;

    protected void Start(){
        UpdateVisuals();
        combatObject.onInit.AddListener(SpawnAdjacentCover);
    }

    void SpawnAdjacentCover(){
        if (adjacentCoverPrefab == null){
            return;
        }

        var targetPos = combatObject.Node.GetPos() + direction.Vector();
        var targetNode = combatObject.Grid().GetNode(targetPos);

        if (targetNode == null){
            return;
        }

        var spawnedHalf = Instantiate(adjacentCoverPrefab);

        var coverComponent = spawnedHalf.GetComponentInChildren<CoverComponent>();
        coverComponent.InitializeAsSpawnedHalf(direction.Opposite(), targetNode);
    }

    public void InitializeAsSpawnedHalf(Direction oppositeDir, CombatGridNode targetNode){
        adjacentCoverPrefab = null;

        direction = oppositeDir;
        UpdateVisuals();

        combatObject.MoveTo(targetNode);
    }

    void UpdateVisuals(){
        if (coverSprite == null){
            return;
        }
        coverSprite.localRotation = Quaternion.Euler(0, 0, direction.Angle());
    }

#if UNITY_EDITOR
    protected void OnValidate(){
        if (combatObject != null && combatObject.OccupiesTile){
            Debug.LogWarning($"CombatObject {combatObject.Name} has a CoverComponent but also occupies a tile.");
        }

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