using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = StringKeys.AssetMenuRecruitIntakeDataBasePath)]
public class RecruitIntakeData : ScriptableObject{
    [SerializeField] int missionsBetweenRecruits = 2;
    [SerializeField] int recruitsPerDelivery = 3;
    
    [SerializeField] [ReadOnly] int missionsCompletedSinceLastDelivery = 0;
    
    [SerializeField] List<RecruitTemplate> possibleTemplates = new();
    
    [FoldoutGroup("Events")] public UnityEvent<RecruitIntakeData> onChanged = new();

    public int MissionsBetweenRecruits => missionsBetweenRecruits;
    public int RecruitsPerDelivery => recruitsPerDelivery;
    public int MissionsCompletedSinceLastDelivery => missionsCompletedSinceLastDelivery;

    public void OnMissionCompleted(ResourcesData resourcesData){
        missionsCompletedSinceLastDelivery++;
        if (missionsCompletedSinceLastDelivery >= missionsBetweenRecruits){
            missionsCompletedSinceLastDelivery = 0;
            DeliverRecruits(resourcesData);
        }
        onChanged.Invoke(this);
    }

    void DeliverRecruits(ResourcesData resourcesData){
for (int i = 0; i < recruitsPerDelivery; i++){
            string randomName = NameGenerator.RandomName();
            RecruitTemplate randomTemplate = possibleTemplates.Random();
    
            SquadMember newRecruit = new SquadMember(
                randomName, 
                randomTemplate.combatPrefab, 
                new List<Equipment>(randomTemplate.startingEquipment),
                randomTemplate.upkeepCost
            );
            
            resourcesData.AddMember(newRecruit);
}
    }
    
    public void ResetData(){
        missionsCompletedSinceLastDelivery = 0;
        onChanged.Invoke(this);
    }
}

[Serializable]
public struct RecruitTemplate{
    public GameObject combatPrefab;
    public List<Equipment> startingEquipment;
    public int upkeepCost;
}
