using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuUnitPrefabAssetRegistryBasePath)]
public class UnitPrefabAssetRegistry : AssetRegistry<GameObject>{
#if UNITY_EDITOR
    protected override string AssetSearchFilter => "t:Prefab";
    protected override bool IsValidAsset(GameObject asset) => asset.GetComponent<Unit>() != null;
#endif
}
