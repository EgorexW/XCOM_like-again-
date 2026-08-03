using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuMissionTypeBasePath)]
public class  Mission : ScriptableObject
{
    public string missionName;
    [TextArea] public string description;
    
    [Tooltip("The MissionInit prefab to spawn when this mission starts.")]
    public MissionInit missionInitPrefab;
}
