using Sirenix.OdinInspector;
using UnityEngine;

public class SquadSelectionUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] SquadSelection squadSelection;

    [BoxGroup("References")] [Required] [SerializeField] SquadUI squadUI;
    [BoxGroup("References")] [Required] [SerializeField] ResourcesUI resourcesUI;

    Equipment pendingEquipment;

    protected void Awake(){
        squadUI.onEquipmentClicked.AddListener(OnSquadUIEquipmentSquadClicked);
        squadUI.onSquadMemberButtonClicked.AddListener(OnSquadUISquadMemberSquadClicked);
        resourcesUI.onSquadMemberClicked.AddListener(OnSquadUISquadMemberResourcesClicked);
        resourcesUI.onSquadMemberPortraitClicked.AddListener(OnSquadUISquadMemberPortraitResourcesClicked);
        resourcesUI.onEquipmentClicked.AddListener(OnSquadUIEquipmentResourcesClicked);
        squadUI.onSquadMemberPortraitClicked.AddListener(OnSquadUISquadMemberPortraitClicked);
    }

    void OnSquadUISquadMemberPortraitClicked(CampaignState arg0, SquadMember arg1){
        if (pendingEquipment == null){
            return;
        }
        squadSelection.AddEquipmentToSquadMemeber(arg1, pendingEquipment);
        pendingEquipment = null;
    }

    protected void Start(){
        squadUI.ShowSquad(squadSelection.CampaignState);
        resourcesUI.ShowResources(squadSelection.CampaignState);
    }

    void OnSquadUIEquipmentSquadClicked(CampaignState arg0, Equipment arg1, SquadMember arg2){
        squadSelection.RemoveEquipmentFromSquadMemeber(arg2, arg1);
    }

    void OnSquadUISquadMemberSquadClicked(CampaignState arg0, SquadMember arg1){
        // Debug.Log($"Clicked {arg1.Name} in Squad! Removing from Squad.");
        squadSelection.RemoveMemberFromSquad(arg1);
    }

    void OnSquadUISquadMemberResourcesClicked(CampaignState arg0, SquadMember arg1){
        // The main button (remove button) now fires them completely
        arg0.FireMember(arg1);
    }

    void OnSquadUISquadMemberPortraitResourcesClicked(CampaignState arg0, SquadMember arg1){
        // The portrait puts them into the squad
        squadSelection.AddMemberToSquad(arg1);
    }

    void OnSquadUIEquipmentResourcesClicked(CampaignState arg0, Equipment arg1){
        pendingEquipment = arg1;

        Debug.Log($"Grabbed {arg1.name}! Now click a Squad Member to equip.");
    }
}