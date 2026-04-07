using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation", menuName = "Egorex/Animation")]
public class Animation : ScriptableObject{
    public AnimationCell[] animationCells;
#if UNITY_EDITOR
    [OnValueChanged("SetCellsDuration")] [SerializeField] float defaultCellDuration;
#endif
    public bool loop = true;

    [HideIf("loop")] public Sprite spriteOnEnd;

    float cycleDuration = -10;

#if UNITY_EDITOR
    void SetCellsDuration(){
        var cellDuration = defaultCellDuration;
        for (var i = 0; i < animationCells.Length; i++) animationCells[i].duration = cellDuration;
    }
#endif

    public Sprite GetSprite(float timePlaying){
        if (timePlaying > GetCycleDuration() && !loop){
            return spriteOnEnd;
        }
        timePlaying = timePlaying % GetCycleDuration();
        var index = -1;
        while (timePlaying > 0){
            index++;
            timePlaying -= animationCells[index].duration;
        }
        return animationCells[index].sprite;
    }

    public float GetCycleDuration(){
        if (cycleDuration > 0){
            return cycleDuration;
        }
        cycleDuration = 0;
        foreach (var animationCell in animationCells) cycleDuration += animationCell.duration;
        return cycleDuration;
    }
}