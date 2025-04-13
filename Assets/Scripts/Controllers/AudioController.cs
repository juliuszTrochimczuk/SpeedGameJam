using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Controllers
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds;

        public static AudioController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public void PlaySound(string soundName) => GetSound(soundName)?.Play();

        public void StopSound(string soundName) => GetSound(soundName)?.Stop();

        private Sound GetSound(string soundName) => sounds.Find((s) => s.name == soundName);

        [System.Serializable]
        public class Sound
        {
            public string name;
            public bool loop;
            [Range(0.0f, 1.0f)] public float volume;
            [Range(0.1f, 3.0f)] public float pitch;

            public bool IsPlaying => source.isPlaying;

            public AudioClip clip;
            public AudioMixerGroup mixer;

            private AudioSource source;

            public void Init(GameObject holder)
            {
                source = holder.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = volume;
                source.pitch = pitch;
                source.loop = loop;
                source.outputAudioMixerGroup = mixer;
            }

            public void Play() => source.Play();

            public void Stop() => source.Stop();
        }
    }
}