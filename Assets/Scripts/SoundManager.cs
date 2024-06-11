using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    //TODO: Honking when collision,some music,victory sound,car crash sound for obstacles and hitting cars
    //TODO: also for touching buttons
    
}
