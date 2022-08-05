using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("bgm")]
    public AudioClip ingameBgm;

    [Header("sfx")]
    public AudioClip examSfx;

    private void Start()
    {
        PlayBgmSound(ingameBgm, 0.1f);
    }

    public void PlayBgmSound(AudioClip clip, float volume = 0.3f)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = volume;
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

    public void PlaySfxSound(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}
