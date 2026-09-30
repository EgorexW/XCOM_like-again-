using Sirenix.OdinInspector;
using UnityEngine;

public class CreateExplosionHazardZone : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] ExplosionEffect explosionEffect;
    
    [Required][SerializeField] GameObject hazardPrefab;
    [SerializeField] HazardSettings hazardSettings;

    ICombatObject hazardCombatObject;

    protected void Awake(){
        var combatObject = GetComponent<ICombatObject>();
        combatObject.onInit.AddListener(CreateHazard);
        combatObject.onRemove.AddListener(RemoveHazard);
    }

    void RemoveHazard(ICombatObject arg0){
        if (hazardCombatObject != null){
            hazardCombatObject.Remove();
        }
    }

    public void CreateHazard(ICombatObject combatObject){
        var centerNode = combatObject.GetCenterNode();
        var nodes = explosionEffect.GetAffectedNodes(centerNode);
        hazardCombatObject = centerNode.Spawn(hazardPrefab);
        hazardCombatObject.MoveTo(nodes);
        var hazardComponent = hazardCombatObject.GetCombatComponent<HazardComponent>();
        hazardComponent.settings = hazardSettings;
    }
}