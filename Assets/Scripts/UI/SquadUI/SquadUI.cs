using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SquadUI : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] SquadSelection squadSelection;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;
    
    SquadData squad;

    void Awake(){
        ShowSquad(squadSelection.GetSquad());
    }

    public void ShowSquad(SquadData squadTmp){
        squad = squadTmp;
        var count = squad.SquadMembers.Count;
        objectsPool.SetCount(count);
        for (int i = 0; i < count; i++){
            var member = squad.SquadMembers[i];
            var obj = objectsPool.GetActiveObject(i);
            var squadSlotUI = obj.GetComponent<SquadMemberUI>();
            squadSlotUI.Show(member);
        }
    }
}
