using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEndCombat : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;
    [BoxGroup("References")] [Required] [SerializeField] CombatReportCreator combatReportCreator;
    [SerializeField] string afterCombatScene;

    protected void Awake(){
        combatSystem.onCombatEnded.AddListener(OnCombatEnded);
    }

    void OnCombatEnded(){
        Debug.Log("Combat Ended!");
        combatReportCreator.CreateReport();
        SceneManager.LoadScene(afterCombatScene);
    }
}