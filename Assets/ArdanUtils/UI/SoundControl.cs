using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public static SoundControl instance;

    // public AudioSource music_source;
    public AudioSource[] sound_sources;
    private Queue<AudioSource> queue_sources;

    public AudioClip click, spawn, fireball, takedamage, takedamage_bowing, bowing, merge, drop, collect_coin, lose, win, coin, newlevel, reward_click,
        enemy_spawn, machine_destroy;

    public AudioClip[] animal_dies, human_dies;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            queue_sources = new Queue<AudioSource>(sound_sources);
        }
    }

    public void ChangeSoundVolume()
    {
        foreach (var sound in sound_sources)
        {
          // sound.mute = UIController.instace.sound == 0;
        }
    }

    public void ChangeMusicVolume()
    {
        // music_source.mute = UIController.instace.music == 0;
    }

    public void PlayShot(AudioClip clip, float volume = 0.7f)
    {
        //if (UIController.instace.sound == 0)
        {
            //return;
        }
        var source = queue_sources.Dequeue();
        source.volume = volume;
        source.PlayOneShot(clip);
        queue_sources.Enqueue(source);
    }
}