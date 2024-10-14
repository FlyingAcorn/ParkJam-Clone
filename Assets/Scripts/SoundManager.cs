using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    private AudioClip _sfx; 
    [SerializeField] private AudioClip[] audioClips;
    //TODO: Honking when collision,some music,victory sound also add confetti,car crash sound for obstacles and hitting cars
    //TODO: also for touching buttons
    // 0 honk,1 crash,2 win,3 click ui

    public void PlaySfx(int clip)
    {
        if (audioSource.isPlaying && audioClips[clip] == _sfx) return;
        _sfx = audioClips[clip];
        audioSource.PlayOneShot(_sfx);
        Debug.Log("playing sound");

    }
    
}
