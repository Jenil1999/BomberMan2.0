using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // SoundManager

    public SoundAudioClip[] allAudioClip;

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }


    public SoundBGM[] allBGM;

    [System.Serializable]
    public class SoundBGM
    {
        public BGMSound sound;
        public AudioClip audioClip;
    }


    public enum Sound
    {
        BombPlant,
        BombExplosion,
        PickItem,
        Death,
        EnemyDeath,
        GameWin,
    }

    public enum BGMSound
    {
        MainMenu,
        MainGame,
        GameOver,
        Loading,
    }

    public AudioSource OneShotAudioSource;
   
    public AudioSource BGMAudioSource;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBGM(BGMSound bGMSound)
    {
        BGMAudioSource.clip = GetBgm(bGMSound);
        BGMAudioSource.Play();
        BGMAudioSource.loop = true;
    }

    public void PlaySound(Sound sound)
    {
        OneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    AudioClip GetBgm(BGMSound _sound)
    {
        foreach (SoundBGM sound in allBGM)
        {
            if (sound.sound == _sound)
            {
                return sound.audioClip;
            }
        }
        return null;
    }

    AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip SoundClip in allAudioClip)
        {
            if (SoundClip.sound == sound)
            {
                return SoundClip.audioClip;
            }
        }
        return null;
    }
}


