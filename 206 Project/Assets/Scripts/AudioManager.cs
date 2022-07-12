using UnityEngine.Audio;
using UnityEngine;
using System;

//Manages certain audio elements within the game
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    //Singleton Pattern
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            //s.source.outputAudioMixerGroup = s.group;
        }
    }

    void Start()
    {
        Play("MainTheme");
    }

    //Finds and plays the specified Audio in the array of sounds
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }
}

