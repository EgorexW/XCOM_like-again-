using Sirenix.OdinInspector;
using UnityEngine;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CombatInit combatInit;

    void Start(){
        var currentLevel = FindAnyObjectByType<Level>();
        if (currentLevel == null){
            Debug.LogError("GameplayInit: No Level found in the scene. Please ensure a Level component is present.");
            return;
        }
        combatInit.InitCombatSystem(currentLevel);
    }
}