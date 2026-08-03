using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuCampaignSequenceBasePath)]
public class CampaingSequence : ScriptableObject{
    [SerializeField] List<Mission> missions;
    
    // For now only that
}