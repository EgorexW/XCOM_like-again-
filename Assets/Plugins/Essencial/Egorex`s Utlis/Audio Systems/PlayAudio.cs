using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class PlayAudio : MonoBehaviour{
    [GetComponent] [SerializeField] AudioSource audioSource;

    [Required] [SerializeField] [InlineEditor] public Sound sound;

    [SerializeField] bool playOnStart;
    [SerializeField] float delayBetweenPlays;

    float lastPlayTime;

    public bool IsPlaying => audioSource.isPlaying;

    void Reset(){
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start(){
        if (playOnStart){
            Play();
        }
    }

    [Button]
    public void Play(){
        if (Time.time - lastPlayTime < delayBetweenPlays){
            return;
        }
        if (audioSource.isPlaying && !sound.canOverride){
            return;
        }
        lastPlayTime = Time.time;
        audioSource.loop = sound.loop;
        audioSource.outputAudioMixerGroup = sound.mixerGroup;
        audioSource.clip = sound.GetClip();
        audioSource.volume = sound.volume * (1f + Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
        audioSource.pitch = sound.pitch * (1f + Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));
        audioSource.Play();
    }

    public void Stop(){
        audioSource.Stop();
    }
}