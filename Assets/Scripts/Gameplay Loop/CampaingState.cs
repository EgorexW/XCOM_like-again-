using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class CampaignState
{
    [SerializeField] MembersState membersState;
    [SerializeField] EquipmentState equipmentState;
    [FormerlySerializedAs("currencyState")] [SerializeField] CurrencyState moneyState;
    [SerializeField] SquadState squadState;
    [SerializeField] MissionsState missionsState;

    public event Action onChanged;
    
    public MembersState Members => membersState;
    public EquipmentState Equipment => equipmentState;
    public CurrencyState Money => moneyState;
    public SquadState Squad => squadState;
    public MissionsState Missions  => missionsState;

    public CampaignState(){
        membersState = new MembersState();
        equipmentState = new EquipmentState();
        moneyState = new CurrencyState();
        squadState = new SquadState();
        missionsState = new MissionsState();
        Init();
    }

    public void Init(){
        membersState!.onChanged += () => onChanged?.Invoke();
        equipmentState!.onChanged += () => onChanged?.Invoke();
        moneyState!.onChanged += () => onChanged?.Invoke();
        squadState!.onChanged += () => onChanged?.Invoke();
        missionsState!.onChanged += () => onChanged?.Invoke();
    }

    public void DieMember(SquadMember member){
        Members.DieMember(member);
    }

    public void FireMember(SquadMember squadMember){
        Members.RemoveMember(squadMember);
    }
}