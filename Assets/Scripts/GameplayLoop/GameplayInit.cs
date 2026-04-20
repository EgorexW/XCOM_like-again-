using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CombatInit combatInit;
    [BoxGroup("References")][Required][SerializeField] LevelInit levelInit;
    [BoxGroup("References")] [Required] [SerializeField] GameObject levelPrefab;

    [SerializeField] Vector2 levelSpawnPos = new Vector2(50, 50);

    void Start(){
        var spawnedLevel = Instantiate(levelPrefab, levelSpawnPos, Quaternion.identity);
        var currentLevel = spawnedLevel.GetComponent<Level>();
        if (currentLevel == null){
            Debug.LogError("GameplayInit: No Level found in the prefab. Please ensure a Level component is present.");
            return;
        }
        CombatContent content = levelInit.InitLevel(currentLevel);
        combatInit.InitCombatSystem(content);
    }
}