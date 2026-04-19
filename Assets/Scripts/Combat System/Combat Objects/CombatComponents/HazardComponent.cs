using System;
using UnityEngine;

[Serializable]
public class HazardSettings{ [SerializeField] public HazardFlags hazardFlags;
    [Range(0, 1)][SerializeField] public float intensity;
    
}

public class HazardComponent : CombatComponent{
    public HazardSettings settings;
    
    public HazardFlags HazardFlags => settings.hazardFlags;
    public float Intensity => settings.intensity;
}



public enum HazardFlags{
    None = 0,
    DamageSoon  = 1 << 0,
}