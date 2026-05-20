using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class Tooltip : MessageUI{
    [SerializeField] [GetComponent] LayoutElement layoutElement;
    [SerializeField] [GetComponent] RectTransform rectTransform;

    protected void Awake(){
        Hide();
    }

    protected void Update(){
        transform.position = General.GetMousePos();

        var desiredWidth = Mathf.Max(headerText.preferredWidth, descriptionText.preferredWidth);
        layoutElement.enabled = desiredWidth > layoutElement.preferredWidth;

        rectTransform.anchoredPosition =
            new Vector2(Mathf.Max(rectTransform.sizeDelta.x, rectTransform.anchoredPosition.x),
                Mathf.Max(rectTransform.sizeDelta.y, rectTransform.anchoredPosition.y));
    }

    public override void Show(){
        base.Show();
        Update();
    }
}