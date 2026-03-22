using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Egorex/Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] public List<AudioClip> clips;

    [Range(0f, 1f)] public float volume = .75f;

    [Range(0f, 1f)] public float volumeVariance = .1f;

    [Range(.1f, 3f)] public float pitch = 1f;

    [Range(0f, 1f)] public float pitchVariance = .1f;

    public bool loop;
    public bool canOverride = true;

    public AudioMixerGroup mixerGroup;

    public virtual AudioClip GetClip()
    {
        return clips.Random();
    }
}