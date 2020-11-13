using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sounds
    {
        BirdJump,
        Score,
        Lose,
        ButtonClick,
    }

    public static void playSound(Sounds sound)
    {
        GameObject gameobject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audiosource = gameobject.GetComponent<AudioSource>();
        audiosource.PlayOneShot(getAudioClip(sound));
    }

    static AudioClip getAudioClip(Sounds sound)
    {
        foreach (GameAssets.soundClip soundclip in GameAssets.GetInstance().soundClips)
        {
            if (soundclip.sound == sound)
            {
                return soundclip.audioClip;
            }
        }
        return null;
    }
}
