using Sirenix.OdinInspector;
using UnityEngine;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatInit combatInit;
    [BoxGroup("References")] [Required] [SerializeField] LevelInit levelInit;
    [BoxGroup("References")] [Required] [SerializeField] CombatContentInit contentInit;

    void Start(){
        var content = contentInit.Init();
        levelInit.InitLevel(content);
        combatInit.InitCombatSystem(content);
    }
}