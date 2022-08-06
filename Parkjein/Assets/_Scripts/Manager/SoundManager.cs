using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        [Header("bgm")]
        public AudioClip ingameBgm;

        [Header("sfx")]
        public AudioClip throwSfx;
        public AudioClip hitSfx;
        public AudioClip knockoutSfx;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Instance.Init();
        }

        private void Start()
        {
            //PlayBgmSound(ingameBgm);
        }

        public void PlayBgmSound(AudioClip clip)
        {
            if (bgmSource.isPlaying) bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        public void ChangeBgmSound(float volume = 0.3f)
        {
            bgmSource.volume = volume;
        }

        public void ChangeSfxSound(float volume = 0.3f)
        {
            sfxSource.volume = volume;
        }

        public void PlaySfxSound(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void PlayHit(bool isKnockout)
        {
            PlaySfxSound(isKnockout ? knockoutSfx : hitSfx);
        }

        private void Init()
        {

        }
    }
}

