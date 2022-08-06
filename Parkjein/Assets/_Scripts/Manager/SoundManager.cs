using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        [Header("bgm")]
        public AudioClip matchBgm;
        public AudioClip selectSkillBgm;
        public AudioClip ingameBgm;

        [Header("sfx")]
        public AudioClip throwSfx;
        public AudioClip hitSfx;
        public AudioClip knockoutSfx;
        public AudioClip killSfx;
        public AudioClip shieldSfx;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Instance.Init();
        }

        private void Start()
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if(scene.name.Equals("ConnectionScene"))
                {
                    PlayMatchBgm();
                }
            };

            PlayMatchBgm();
        }

        #region BGM
        private void PlayBgmSound(AudioClip clip)
        {
            if (bgmSource.isPlaying) bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        public void PlayMatchBgm()
        {
            PlayBgmSound(matchBgm);
        }

        public void PlaySelectSkillBgm()
        {
            PlayBgmSound(selectSkillBgm);
        }

        public void PlayIngameBgm()
        {
            PlayBgmSound(ingameBgm);
        }

        public void ChangeBgmSound(float volume = 0.3f)
        {
            bgmSource.volume = volume;
        }
        #endregion



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

