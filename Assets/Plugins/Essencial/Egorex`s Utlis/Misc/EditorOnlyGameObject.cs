using UnityEngine;

public class EditorOnlyGameObject : MonoBehaviour
{
    readonly bool destroy = false;

    void Awake()
    {
        if (destroy){
            Destroy(gameObject);
            return;
        }
        gameObject.SetActive(false);
    }
}