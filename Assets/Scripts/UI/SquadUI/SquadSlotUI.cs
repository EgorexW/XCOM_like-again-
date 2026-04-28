using Sirenix.OdinInspector;
using UnityEngine;

public class SquadSlotUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] SquadMemberUI squadMemberUI;
    [BoxGroup("References")][Required][SerializeField] UIElement empty;
    
    public void ShowMember(SquadMember squadMemberTmp){
        base.Show();
        squadMemberUI.Show(squadMemberTmp);
        empty.Hide();
    }

    public void ShowEmpty(){
        base.Show();
        squadMemberUI.Hide();
        empty.Show();
    }
}