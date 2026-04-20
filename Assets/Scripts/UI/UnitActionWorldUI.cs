using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

class UnitActionWorldUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshPro actionNameText;

    [SerializeField] Vector2 offset = Vector2.up * 0.5f;
    [SerializeField] Vector2 move = Vector2.up * 0.5f;
    [SerializeField] Vector2 pushMove = Vector2.up;
    [SerializeField] float duration = 3f;
    [SerializeField] float fadeDelay = 2.5f;
    [SerializeField] float overlapRadius = 0.5f;

    Color startColor;
    float startTime;

    void Awake(){
        startColor = actionNameText.color;
    }

    public void ShowAction(UnitAction action){
        base.Show();
        actionNameText.text = action.ActionInfo.Name;
        transform.position = action.unit.GetCenter() + offset;
        actionNameText.color = startColor;
        startTime = Time.time;
    }

    void Update(){
        if (!IsVisible){
            return;
        }
        transform.position += (Vector3)move * Time.deltaTime;
        var time = Time.time - startTime;
        if (time > fadeDelay){
            var t = (time - fadeDelay) / (duration - fadeDelay);
            actionNameText.color = Color.Lerp(startColor, Color.clear, t);
        }
        if (time > duration){
            Hide();
        }
    }

    public void MakeSpace(float distance){
        if (distance < overlapRadius){
            transform.position += (Vector3)pushMove;
        }
    }
}