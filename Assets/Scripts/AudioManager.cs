using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [SerializeField] List<SoundEffect> soundEffects;
    Dictionary<SoundEffectName, AudioClip> dicSounds;
    [System.Serializable]
    public class SoundEffect
    {
        public SoundEffectName name;
        public AudioClip sound;

    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitDictionary();
        SettingVolume();
    }

    private void InitDictionary()
    {
        if (soundEffects == null || soundEffects.Count <= 0) { return; }
        dicSounds = new Dictionary<SoundEffectName, AudioClip>();
        foreach (var soundEffect in soundEffects)
        {
            dicSounds.Add(soundEffect.name, soundEffect.sound);
        }
    }

    public void SettingVolume()
    {
        soundEffectSource.volume = Pref.Volume;
        musicSource.volume = Pref.Volume;
    }
    public void PlaySound(SoundEffectName name)
    {
        AudioClip sound = dicSounds[name];
        if (sound != null)
        {
            soundEffectSource.PlayOneShot(sound);
        }
    }
    public void PlayMusic(AudioClip mClip)
    {
        if (mClip != null)
        {
            musicSource.clip = mClip;
            musicSource.Play();
        }
    }
    public void PlayUIClick()
    {
        AudioClip sound = dicSounds[SoundEffectName.UIClick];
        if (sound != null)
        {
            soundEffectSource.PlayOneShot(sound);
        }
    }
}
public enum SoundEffectName
{
    ShuffleCard,
    DealButton,
    FailedMove,
    FailedUnlock,
    Unlock,
    Win,
    UIClick,
    GetScore
}
