using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    private AudioClip _sfx; 
    [SerializeField] private AudioClip[] audioClips;
    // 0 honk,1 crash,2 win,3 click ui
    public void PlaySfx(int clip)
    {
        if (audioSource.isPlaying && audioClips[clip] == _sfx) return;
        _sfx = audioClips[clip];
        audioSource.PlayOneShot(_sfx);
    }
    
}
