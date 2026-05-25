using UnityEngine;
using Sirenix.OdinInspector;

class LevelInit : MonoBehaviour{
    [Header("DEPRECATED: Copy variables to new objects and delete this script")]
    [SerializeField] Vector2 levelSpawnPos = new(50, 50);

    // public void InitLevel(CombatContent content) is now handled by MissionInit
}