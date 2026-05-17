using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class Tooltip : UIElement{
    [SerializeField] [GetComponent] LayoutElement layoutElement;
    [SerializeField] [GetComponent] RectTransform rectTransform;
    [SerializeField] [Required] TextMeshProUGUI headerText;
    [SerializeField] [Required] TextMeshProUGUI descriptionText;

    [SerializeField] bool moveToMouse;

    protected void Awake(){
        Hide();
    }

    protected void Update(){
        if (!moveToMouse){
            return;
        }
        transform.position = General.GetMousePos();

        var desiredWidth = Mathf.Max(headerText.preferredWidth, descriptionText.preferredWidth);
        layoutElement.enabled = desiredWidth > layoutElement.preferredWidth;

        rectTransform.anchoredPosition =
            new Vector2(Mathf.Max(rectTransform.sizeDelta.x, rectTransform.anchoredPosition.x),
                Mathf.Max(rectTransform.sizeDelta.y, rectTransform.anchoredPosition.y));
    }

    void Show(string header, string description){
        base.Show();
        headerText.text = header;
        descriptionText.text = description;
        Update();
    }

    public void Show(Message message){
        Show(message.header, message.description);
    }
}