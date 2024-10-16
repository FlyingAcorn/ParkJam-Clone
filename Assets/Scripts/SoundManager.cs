using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource sfxSource;
    public AudioSource backgroundSource;
    private AudioClip _sfx;
    private AudioClip _background;
    [SerializeField] private AudioClip[] audioClips;// 0 honk,1 crash,2 win,3 click ui
    [SerializeField] private AudioClip[] backgroundClips; //0 menu sound, 1 ingame

    
    
    public void PlaySfx(int clip)
    {
        if (sfxSource.isPlaying && audioClips[clip] == _sfx) return;
        _sfx = audioClips[clip];
        sfxSource.PlayOneShot(_sfx);
    }
    public void PlayBackgroundSound(int clip)
    {
        _background = backgroundClips[clip];
        backgroundSource.clip = _background;
        backgroundSource.Play();
    }
    
}
