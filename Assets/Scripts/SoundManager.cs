using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        LoadVolumeSettings();
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudio.clip = clip;
        sfxAudio.PlayOneShot(clip, sfxVolume * 0.1f);
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

    [System.Serializable]
    class VolumeSettingsData // new class that stores name and score of highscore player
    {
        public float SFXVolume;
        public float MusicVolume;
    }

    public void SaveVolumeSettings() // writes highscore data class to a json file
    {
        VolumeSettingsData data = new VolumeSettingsData();
        data.SFXVolume = sfxVolume;
        data.MusicVolume = musicVolume;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/settingsfile.json", json);
    }

    public void LoadVolumeSettings() // reads highscore data class from a json file
    {
        string path = Application.persistentDataPath + "/settingsfile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            VolumeSettingsData data = JsonUtility.FromJson<VolumeSettingsData>(json);

            sfxVolume = data.SFXVolume;
            musicVolume = data.MusicVolume;
        }
    }

}


