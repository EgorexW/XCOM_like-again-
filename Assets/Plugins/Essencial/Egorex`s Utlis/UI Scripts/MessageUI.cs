using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MessageUI : UIElement{
    [SerializeField] [Required] protected TextMeshProUGUI headerText;
    [SerializeField] [Required] protected TextMeshProUGUI descriptionText;

    void Show(string header, string description){
        base.Show();
        headerText.text = header;
        descriptionText.text = description;
    }

    public void Show(Message message){
        Show(message.header, message.description);
    }
}