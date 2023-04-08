using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace WindTurbineVR.Core.Audio
{
    [System.Serializable]
    public class Sound
    {

        [SerializeField]
        public AudioClip clip;

        [Space] [SerializeField] 
        public string name;

        [Range(0f,1f)] [SerializeField] 
        public float volume;
        
        [Range(.1f, 3f)] [SerializeField] 
        public float pitch;

        #region Private
        [HideInInspector] AudioSource source;
        #endregion

        public AudioSource Source 
        {
            get => source;
            set
            {
                source = value;
                source.clip = clip;
                source.volume = volume;
                source.pitch = pitch;
            }
        }
    }
}
