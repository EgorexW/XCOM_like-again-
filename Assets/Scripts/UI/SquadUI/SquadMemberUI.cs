using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SquadMemberUI : UIElement
{
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI nameText;
    [FormerlySerializedAs("removeButton")] [BoxGroup("References")][Required][SerializeField] Button button;
    [BoxGroup("References")][SerializeField] EquipmentTypesUI equipmentUI;
    
    [FoldoutGroup("Events")] public UnityEvent<SquadMember> onClicked;
    [FoldoutGroup("Events")] public UnityEvent<SquadMember, Equipment> onEquipmentClicked;
    
    SquadMember squadMember;

    void Awake(){
        button.onClick.AddListener(OnButtonClicked);
        equipmentUI?.onEquipmentTypeClicked.AddListener(OnEquipmentTypeClicked);
    }

    void OnButtonClicked(){
        onClicked.Invoke(squadMember);
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