using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SquadUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    [FormerlySerializedAs("onSquadMemberClicked")] [FoldoutGroup("Events")]
    public UnityEvent<SquadData, SquadMember> onSquadMemberButtonClicked = new();

    [FoldoutGroup("Events")] public UnityEvent<SquadData, Equipment, SquadMember> onEquipmentClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<SquadData, SquadMember> onSquadMemberPortraitClicked = new();

    SquadData squad;

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
        onSquadMemberPortraitClicked.Invoke(squad, arg0);
    }

    void OnSquadMemberUIEquipmentClicked(SquadMember arg0, Equipment arg1){
        onEquipmentClicked.Invoke(squad, arg1, arg0);
    }

    void OnSquadMemberUIClicked(SquadMember arg0){
        // Debug.Log($"Clicked {arg0.Name} in Squad!");
        onSquadMemberButtonClicked.Invoke(squad, arg0);
    }

    public void ShowSquad(SquadData squadTmp){
        base.Show();
        RemoveSquad();
        squad = squadTmp;
        squad.onChanged.AddListener(OnSquadChanged);
        UpdateSquad();
    }

    void UpdateSquad(){
        var count = squad.SquadMembers.Count;
        objectsPool.SetCount(count);
        for (var i = 0; i < count; i++){
            var member = squad.SquadMembers[i];
            var obj = objectsPool.GetActiveObject(i);
            var squadSlotUI = obj.GetComponent<SquadMemberUI>();
            squadSlotUI.Show(member);
        }
    }

    void OnSquadChanged(SquadData arg0){
        UpdateSquad();
    }

    public override void Hide(){
        base.Hide();
        RemoveSquad();
    }

    void RemoveSquad(){
        squad?.onChanged.RemoveListener(OnSquadChanged);
        squad = null;
    }
}