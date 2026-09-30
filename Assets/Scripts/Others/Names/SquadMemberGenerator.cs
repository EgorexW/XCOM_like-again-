using System;
using System.Collections.Generic;
using UnityEngine;

public class SquadMemberGenerator : ScriptableObject{
    [SerializeField] List<RecruitTemplate> possibleTemplates = new();
    
    
    public void GenerateRecruits(int amount){
        for (int i = 0; i < amount; i++){
            GenerateRecruit();
        }
    }

    void GenerateRecruit(){
        string randomName = NameGenerator.RandomName();
        RecruitTemplate randomTemplate = possibleTemplates.Random();
    
        SquadMember newRecruit = new SquadMember(
            randomName, 
            randomTemplate.combatPrefab, 
            new List<Equipment>(randomTemplate.startingEquipment),
            randomTemplate.upkeepCost
        );
    }
}

[Serializable]
public struct RecruitTemplate{
    public GameObject combatPrefab;
    public List<Equipment> startingEquipment;
    public int upkeepCost;
}

    