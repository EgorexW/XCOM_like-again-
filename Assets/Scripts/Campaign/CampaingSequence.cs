using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuCampaignSequenceBasePath)]
public class CampaingSequence : ScriptableObject{
    [SerializeField] List<CampaignStep> steps;

    public CampaignStep GetStep(int progressionStep){
        if (progressionStep >= 0 && progressionStep < steps.Count){
            return steps[progressionStep];
        }
        Debug.LogError($"Invalid progression step: {progressionStep}. Steps count: {steps.Count}");
        return null;
    }
}

// DAY
[Serializable]
public class CampaignStep{
    [SerializeField] Mission mission;
    
    // TODO Shop Unlocks
    
    public Mission Mission => mission;
}