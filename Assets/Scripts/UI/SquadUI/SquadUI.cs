using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SquadUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    [FormerlySerializedAs("onSquadMemberClicked")] [FoldoutGroup("Events")]
    public UnityEvent<CampaignState, SquadMember> onSquadMemberButtonClicked = new();

    [FoldoutGroup("Events")] public UnityEvent<CampaignState, Equipment, SquadMember> onEquipmentClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<CampaignState, SquadMember> onSquadMemberPortraitClicked = new();

    CampaignState campaignState;

    protected void Awake(){
        objectsPool.onCreateObject.AddListener(OnCreateSquadMemberUI);
    }

    void OnCreateSquadMemberUI(GameObject arg0){
        var squadMemberUI = arg0.GetComponent<SquadMemberUI>();
        squadMemberUI.onButtonClicked.AddListener(OnSquadMemberUIClicked);
        squadMemberUI.onEquipmentClicked.AddListener(OnSquadMemberUIEquipmentClicked);
        squadMemberUI.onPortraitClicked.AddListener(OnSquadMemberUIPortraitClicked);
    }

    void OnSquadMemberUIPortraitClicked(SquadMember arg0){
        onSquadMemberPortraitClicked.Invoke(campaignState, arg0);
    }

    void OnSquadMemberUIEquipmentClicked(SquadMember arg0, Equipment arg1){
        onEquipmentClicked.Invoke(campaignState, arg1, arg0);
    }

    void OnSquadMemberUIClicked(SquadMember arg0){
        // Debug.Log($"Clicked {arg0.Name} in Squad!");
        onSquadMemberButtonClicked.Invoke(campaignState, arg0);
    }

    public void ShowSquad(CampaignState squadTmp){
        base.Show();
        RemoveSquad();
        campaignState = squadTmp;
        campaignState.onChanged += UpdateCampaignState;
        UpdateCampaignState();
    }

    void UpdateCampaignState(){
        var count = campaignState.Squad.Members.Count;
        objectsPool.SetCount(count);
        for (var i = 0; i < count; i++){
            var member = campaignState.Squad.Members[i];
            var obj = objectsPool.GetActiveObject(i);
            var squadSlotUI = obj.GetComponent<SquadMemberUI>();
            squadSlotUI.Show(member);
        }
    }

    void OnSquadChanged(CampaignState arg0){
        UpdateCampaignState();
    }

    public override void Hide(){
        base.Hide();
        RemoveSquad();
    }

    void RemoveSquad(){
        if (campaignState != null){
            campaignState.onChanged -= UpdateCampaignState;
        }
        campaignState = null;
    }
}