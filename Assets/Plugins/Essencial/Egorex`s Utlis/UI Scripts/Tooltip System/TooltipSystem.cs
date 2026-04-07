using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class TooltipSystem : MonoBehaviour{
    static TooltipSystem i;
    [SerializeField] [Required] Tooltip tooltip;

    protected void Awake(){
        i = this;
    }

    public static void Hide(){
        if (i == null){
            return;
        }
        i.tooltip.Hide();
    }

    public static void Show(Message message){
        i.tooltip.Show(message);
    }
}

[Serializable]
public struct Message{
    public string header;
    public string description;

    public Message(string header, string description){
        this.header = header;
        this.description = description;
    }
}