using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "AI Hazard Scoring", fileName = "AI Hazard Scoring", order = 0)]
class AIHazardScoring : ScriptableObject{
    [InfoBox("$EnumInfo")] [SerializeField] List<float> multPerHazardFlag;

    public string EnumInfo => General.EnumDescription<HazardFlags>();
    
    public float GetHazardScore(HazardComponent hazard, AIContext context){
        float highestMultiplier = 0f;
        
        int flagsAsInt = (int)hazard.HazardFlags;
        
        for (int i = 0; i < multPerHazardFlag.Count; i++){
            
            int currentFlagBit = 1 << i;
            
            if ((flagsAsInt & currentFlagBit) != 0){
                if (multPerHazardFlag[i] > highestMultiplier){
                    highestMultiplier = multPerHazardFlag[i];
                }
            }
        }

        // Apply the highest found multiplier to the intensity
        return highestMultiplier * hazard.Intensity;
    }
}