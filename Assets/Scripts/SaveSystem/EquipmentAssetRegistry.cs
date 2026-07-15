using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuEquipmentAssetRegistryBasePath)]
public class EquipmentAssetRegistry : AssetRegistry<Equipment>{
#if UNITY_EDITOR
    protected override string AssetSearchFilter => "t:Equipment";
#endif
}
