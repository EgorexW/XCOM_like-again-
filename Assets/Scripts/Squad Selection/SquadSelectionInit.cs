using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SquadSelectionInit : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] ShopLogic shopLogic;
    
    protected void Awake(){
        Init();
    }

    public void Init(){
        shopLogic.GenerateShop();
    }
}
