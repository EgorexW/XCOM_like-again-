using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] CampaignManager campaignManager;
    
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI title;

    protected void Awake(){
        var mission = campaignManager.GetCurrentMission();
        title.text = mission.missionName;
    }
}
