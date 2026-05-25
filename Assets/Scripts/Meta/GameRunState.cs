using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuGameRunStateBasePath)]
public class GameRunState : ScriptableObject
{
    [Header("Current Run Data")]
    public MissionType selectedMissionType;
    
    // Additional cross-scene variables can be added here in the future
}
