using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadMemberUI : UIElement
{
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI nameText;
    [BoxGroup("References")][Required][SerializeField] Button removeButton;
    
    SquadMember squadMember;

    void Awake(){
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
    }

    void OnRemoveButtonClicked(){
        throw new NotImplementedException();
    }

    public void Show(SquadMember squadMemberTmp){
        base.Show();
        squadMember = squadMemberTmp;
        nameText.text = squadMember.name;
    }
}
