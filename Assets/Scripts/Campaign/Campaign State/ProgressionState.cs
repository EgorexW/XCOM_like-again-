using System;
using UnityEngine;

[Serializable]
public class ProgressionState{
    [SerializeField] CampaignProgressionState campaignProgressionState;
    [SerializeField] int step = 0;
    [SerializeField] CampaignResult campaignResult;

    public CampaignProgressionState State => campaignProgressionState;
    public int Step => step;
    
    public event Action onChanged;

    public void IncrementStep(){
        step++;
        Debug.Log($"Progression step incremented to {step}");
        onChanged?.Invoke();
    }

    public CampaignResult GetCampaignResult(){
        if (campaignProgressionState != CampaignProgressionState.Completed){
            Debug.LogError($"Campaign is not completed yet. Current state: {campaignProgressionState}");
        }
        return campaignResult;
    }
    public void SetCampaignResult(CampaignResult result){
        Debug.Log($"Setting campaign result: Victory: {result.isVictory}, Reason: {result.reason}");
        campaignResult = result;
        campaignProgressionState = CampaignProgressionState.Completed;
        onChanged?.Invoke();
    }
}

public class CampaignResult{
    public bool isVictory;
    public string reason;
}

public enum CampaignProgressionState{
    Ongoing,
    Completed
}