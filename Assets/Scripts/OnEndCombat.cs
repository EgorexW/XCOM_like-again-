using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEndCombat  : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    [SerializeField] string afterCombatScene;
    
    void Awake() {
        combatSystem.onCombatEnded.AddListener(OnCombatEnded);
    }

    void OnCombatEnded(){
        Debug.Log("Combat Ended!");
        SceneManager.LoadScene(afterCombatScene);
    }  
}