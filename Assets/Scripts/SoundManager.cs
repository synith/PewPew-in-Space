using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private AudioSource sfxAudio;
    [SerializeField]private AudioSource bgmAudio;

    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    
    
    public static SoundManager Instance;


    private void Awake()
    {
        if (Instance != null)  // check to make sure there is not another instance of this class
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudio.clip = clip;
        sfxAudio.PlayOneShot(clip, 0.1f);
    }
    public void PlayMusic(AudioClip clip)
    {
        bgmAudio.clip = clip;
        bgmAudio.volume = musicVolume * 0.1f;
        bgmAudio.Play();
    }
    public void StopMusic()
    {
        bgmAudio.Stop();
    }
}
