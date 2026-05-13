using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool membersPool;
    [BoxGroup("References")] [Required] [SerializeField] EquipmentTypesUI equipmentUI;

    ResourcesData resources;

    [FoldoutGroup("Events")] public UnityEvent<ResourcesData, SquadMember> onSquadMemberClicked = new();
    [FoldoutGroup("Events")] public UnityEvent<ResourcesData, Equipment> onEquipmentClicked = new();

    void Awake(){
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

    public void ShowResources(ResourcesData resourcesData){
        resources = resourcesData;
        resources.onChanged.AddListener(OnResourcesChanged);
        UpdateResources();
    }

    void UpdateResources(){
        var count = resources.Members.Count;
        membersPool.SetCount(count);
        for (var i = 0; i < count; i++){
            var member = resources.Members[i];
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
        resources = null;
    }
}