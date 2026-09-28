using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuMissionAssetRegistryBasePath)]
public class MissionAssetRegistry : AssetRegistry<Mission>{
#if UNITY_EDITOR
    protected override string AssetSearchFilter => "t:Mission";
#endif
}
