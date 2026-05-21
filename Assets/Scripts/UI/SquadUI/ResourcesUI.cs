using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool membersPool;
    [BoxGroup("References")] [Required] [SerializeField] EquipmentTypesUI equipmentUI;

    ResourcesData resources;
    SquadData squad;

    [FoldoutGroup("Events")] public UnityEvent<ResourcesData, SquadMember> onSquadMemberClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<ResourcesData, Equipment> onEquipmentClicked = new();

    protected void Awake(){
        membersPool.onCreateObject.AddListener(OnCreateSquadMemberUI);
        equipmentUI.onEquipmentTypeClicked.AddListener(OnEquipmentTypeClicked);
    }

    void OnEquipmentTypeClicked(Equipment arg0){
        onEquipmentClicked.Invoke(resources, arg0);
    }

    void OnCreateSquadMemberUI(GameObject arg0){
        var squadMemberUI = arg0.GetComponent<SquadMemberUI>();
        squadMemberUI.onButtonClicked.AddListener(OnSquadMemberUIClicked);
    }

    void OnSquadMemberUIClicked(SquadMember arg0){
        onSquadMemberClicked.Invoke(resources, arg0);
    }

    public void ShowResources(ResourcesData resourcesData, SquadData squadData = null){
        resources = resourcesData;
        squad = squadData;
        resources.onChanged.AddListener(OnResourcesChanged);
        if (squad != null) squad.onChanged.AddListener(OnSquadChanged);
        UpdateResources();
    }

    void OnSquadChanged(SquadData data){
        UpdateResources();
    }

    void UpdateResources(){
        var availableMembers = new List<SquadMember>();
        foreach (var member in resources.Members){
            if (squad == null || !squad.SquadMembers.Contains(member)){
                availableMembers.Add(member);
            }
        }

        membersPool.SetCount(availableMembers.Count);
        for (var i = 0; i < availableMembers.Count; i++){
            var member = availableMembers[i];
            var obj = membersPool.GetActiveObject(i);
            var resourcesSlotUI = obj.GetComponent<SquadMemberUI>();
            resourcesSlotUI.Show(member);
        }
        equipmentUI.Show(resources.Equipment);
    }

    void OnResourcesChanged(ResourcesData arg0){
        UpdateResources();
    }

    public override void Hide(){
        base.Hide();
        resources?.onChanged.RemoveListener(OnResourcesChanged);
        squad?.onChanged.RemoveListener(OnSquadChanged);
        resources = null;
        squad = null;
    }
}