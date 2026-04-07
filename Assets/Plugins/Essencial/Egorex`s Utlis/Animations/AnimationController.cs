using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour{
    [HideIf("@image != null")] [BoxGroup("References")] [Required] [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [HideIf("@spriteRenderer != null")] [BoxGroup("References")] [Required] [SerializeField] Image image;

    [SerializeField] Optional<Animation> animationOnAwake;

    Animation activeAnimation;
    float timePlaying;

    [FoldoutGroup("Events")] public UnityEvent onNewFrame;

    public Sprite GetSprite(){
        if (spriteRenderer != null){
            return spriteRenderer.sprite;
        }
        if (image != null){
            return image.sprite;
        }
        throw new Exception("No SpriteRenderer or Image assigned");
    }

    public void SetSprite(Sprite sprite){
        if (spriteRenderer != null){
            spriteRenderer.sprite = sprite;
            return;
        }
        if (image != null){
            image.sprite = sprite;
            return;
        }
        throw new Exception("No SpriteRenderer or Image assigned");
    }

    protected void Awake(){
        if (animationOnAwake){
            SetAnimation(animationOnAwake);
        }
    }

    protected void Update(){
        if (activeAnimation == null){
            return;
        }
        timePlaying += Time.deltaTime;
        var prevSprite = GetSprite();
        var newSprite = activeAnimation.GetSprite(timePlaying);
        if (prevSprite != newSprite){
            onNewFrame.Invoke();
        }
        SetSprite(newSprite);
    }

    public void SetAnimation(Animation animation){
        activeAnimation = animation;
        timePlaying = 0;
    }

    public Animation GetAnimation(){
        return activeAnimation;
    }

    public void StopAnimation(){
        activeAnimation = null;
    }
}