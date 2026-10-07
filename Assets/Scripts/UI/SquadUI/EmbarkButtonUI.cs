using System;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EmbarkButtonUI : MonoBehaviour
{
    [BoxGroup("References")][GetComponent][SerializeField] Button embarkButton;
    [BoxGroup("References")][Required][SerializeField] SquadSelectionEmbark squadSelectionEmbark;

    protected void Awake(){
        embarkButton.onClick.AddListener(OnEmbarkButtonClicked);
    }

    //TODO idealy not every frame
    protected void Update(){
        embarkButton.interactable = squadSelectionEmbark.EmbarkValidation() == EmbrakValidation.Valid;
    }

    void OnEmbarkButtonClicked(){
        squadSelectionEmbark.TryEmbark();
    }
}
