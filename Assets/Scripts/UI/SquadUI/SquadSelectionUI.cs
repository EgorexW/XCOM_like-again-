using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SquadSelectionUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] SquadSelection squadSelection;
    
    [BoxGroup("References")][Required][SerializeField] SquadUI squadUI;
    [BoxGroup("References")][Required][SerializeField] ResourcesUI resourcesUI;

    void Awake(){
        squadUI.ShowSquad(squadSelection.Squad);
        resourcesUI.ShowResources(squadSelection.Resources);
        squadUI.onEquipmentClicked.AddListener(OnSquadUIEquipmentSquadClicked);
        squadUI.onSquadMemberClicked.AddListener(OnSquadUISquadMemberSquadClicked);
        resourcesUI.onSquadMemberClicked.AddListener(OnSquadUISquadMemberResourcesClicked);
        resourcesUI.onEquipmentClicked.AddListener(OnSquadUIEquipmentResourcesClicked);
    }

    void OnSquadUIEquipmentSquadClicked(SquadData arg0, Equipment arg1, SquadMember arg2){
        throw new NotImplementedException();
    }

    void OnSquadUISquadMemberSquadClicked(SquadData arg0, SquadMember arg1){
        throw new NotImplementedException();
    }

    void OnSquadUISquadMemberResourcesClicked(ResourcesData arg0, SquadMember arg1){
        throw new NotImplementedException();
    }

    void OnSquadUIEquipmentResourcesClicked(ResourcesData arg0, Equipment arg1){
        throw new NotImplementedException();
    }
}