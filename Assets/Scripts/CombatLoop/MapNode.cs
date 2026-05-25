using UnityEngine;

public enum MapNodeType
{
    FriendlySpawn,
    EnemySpawn,
    ObjectiveLocation
}

public class MapNode : MonoBehaviour
{
    public MapNodeType nodeType;
    public string nodeGroup; // Identifier for group spawning logic
}
