using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

namespace WindTurbineVR.Core.Audio
{
    public class AudioController : MonoBehaviour
    {
        public Sound[] sounds;

        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.Source.Play();
        }
    }
}
