using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuCampaignSequenceBasePath)]
public class CampaingSequence : ScriptableObject{
    [SerializeField] List<CampaignStep> steps;
}

// DAY
[Serializable]
class CampaignStep{
    public Mission mission;
    
    // TODO Shop Unlocks
}