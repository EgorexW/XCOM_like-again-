using Sirenix.OdinInspector;
using UnityEngine;

public class CombatUI : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;

    [BoxGroup("References")] [Required] [SerializeField] GridUI gridUI;

    protected void Awake(){
        combatSystem.onCombatStarted.AddListener(Init);
    }

    void Init(){
        gridUI.ShowGrid(combatSystem.CombatGrid.Grid);
    }
}