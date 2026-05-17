using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SquadMemberUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI nameText;
    [BoxGroup("References")] [Required] [SerializeField] Button button;
    [BoxGroup("References")] [Required] [SerializeField] Button portraitButton;
    [BoxGroup("References")] [SerializeField] EquipmentTypesUI equipmentUI;

    [FormerlySerializedAs("onClicked")] [FoldoutGroup("Events")] public UnityEvent<SquadMember> onButtonClicked;
    [FoldoutGroup("Events")] public UnityEvent<SquadMember, Equipment> onEquipmentClicked;
    [FoldoutGroup("Events")] public UnityEvent<SquadMember> onPortraitClicked = new();

    SquadMember squadMember;

    protected void Awake(){
        button.onClick.AddListener(OnButtonClicked);
        portraitButton.onClick.AddListener(OnPortraitButtonClicked);
        equipmentUI?.onEquipmentTypeClicked.AddListener(OnEquipmentTypeClicked);
    }

    void OnPortraitButtonClicked(){
        // Debug.Log($"Clicked {squadMember.Name}'s portrait!");
        onPortraitClicked.Invoke(squadMember);
    }

    void OnButtonClicked(){
        // Debug.Log($"Clicked {squadMember.Name}!");
        onButtonClicked.Invoke(squadMember);
    }

    public void Show(SquadMember squadMemberTmp){
        base.Show();
        RemoveSquadMember();
        squadMember = squadMemberTmp;
        squadMember.onChanged.AddListener(UpdateSquadMember);
        UpdateSquadMember();
    }

    void OnEquipmentTypeClicked(Equipment arg0){
        onEquipmentClicked.Invoke(squadMember, arg0);
    }

    public override void Hide(){
        base.Hide();
        RemoveSquadMember();
    }

    void RemoveSquadMember(){
        squadMember?.onChanged.RemoveListener(UpdateSquadMember);
        squadMember = null;
    }

    void UpdateSquadMember(SquadMember arg0){
        UpdateSquadMember();
    }

    void UpdateSquadMember(){
        nameText.text = squadMember.Name;
        equipmentUI?.Show(squadMember.Equipment);
    }
}