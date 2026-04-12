using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

class CollapseablePool : CountUI{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool heartsPool;
    [BoxGroup("References")] [Required] [SerializeField] GameObject collapsedUI;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] int collapseAmount = 4;

    public override void SetCount(int value){
        base.SetCount(value);
        healthText.text = value.ToString();
        if (value < collapseAmount){
            heartsPool.SetCount(value);
            collapsedUI.SetActive(false);
        }
        else{
            heartsPool.Clear();
            collapsedUI.SetActive(true);
        }
    }
}