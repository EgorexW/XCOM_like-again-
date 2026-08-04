using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool membersPool;
    [BoxGroup("References")] [Required] [SerializeField] EquipmentTypesUI equipmentUI;

    CampaignState campaignState;

    [FoldoutGroup("Events")] public UnityEvent<CampaignState, SquadMember> onSquadMemberClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<CampaignState, SquadMember> onSquadMemberPortraitClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<CampaignState, Equipment> onEquipmentClicked = new();

    protected void Awake(){
        membersPool.onCreateObject.AddListener(OnCreateSquadMemberUI);
        equipmentUI.onEquipmentTypeClicked.AddListener(OnEquipmentTypeClicked);
    }

    void OnEquipmentTypeClicked(Equipment arg0){
        onEquipmentClicked.Invoke(campaignState, arg0);
    }

    void OnCreateSquadMemberUI(GameObject arg0){
        var squadMemberUI = arg0.GetComponent<SquadMemberUI>();
        squadMemberUI.onButtonClicked.AddListener(OnSquadMemberUIClicked);
        squadMemberUI.onPortraitClicked.AddListener(OnSquadMemberPortraitClicked);
    }

    void OnSquadMemberUIClicked(SquadMember arg0){
        onSquadMemberClicked.Invoke(campaignState, arg0);
    }

    void OnSquadMemberPortraitClicked(SquadMember arg0){
        onSquadMemberPortraitClicked.Invoke(campaignState, arg0);
    }

    public void ShowResources(CampaignState campaignStateTmp){
        campaignState = campaignStateTmp;
        campaignState.onChanged += OnCampaignStateChanged;
        UpdateResources();
    }

    void UpdateResources(){
        var availableMembers = new List<SquadMember>();
        foreach (var member in campaignState.Members.ActiveMembers){
            if (!campaignState.Squad.Members.Contains(member)){
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
        equipmentUI.Show(campaignState.Equipment.Equipment);
    }

    void OnCampaignStateChanged(){
        UpdateResources();
    }

    public override void Hide(){
        base.Hide();
        if (campaignState != null){
            campaignState.onChanged -= OnCampaignStateChanged;
        }
        campaignState = null;
    }
}