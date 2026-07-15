using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class StatsUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] ResourcesData resourcesData;
    
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI moneyText;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI retirementRateText;
    
    
    void UpdateUI(){
        moneyText.text = $"Money: ${resourcesData.Money}";
        retirementRateText.text = $"Retirement Rate: {resourcesData.RetirementRate():P1}";
    }

    void Start(){
        resourcesData.onChanged.AddListener(OnResourcesChanged);
        UpdateUI();
    }

    void OnResourcesChanged(ResourcesData arg0){
        UpdateUI();
    }

    public override void Show(){
        base.Show();
        UpdateUI();
    }
    
    protected void OnDestroy() {
        if (resourcesData != null) {
            resourcesData.onChanged.RemoveListener(OnResourcesChanged);
        }
    }
}
