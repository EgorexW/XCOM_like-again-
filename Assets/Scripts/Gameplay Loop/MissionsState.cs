using System;
using UnityEngine;

[Serializable]
public class MissionsState{
    [SerializeField] Mission selectedMission;
    
    public Mission SelectedMission => selectedMission;
    
    public event Action onChanged;
}