using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicClip
{
    NONE = -1,
    MENU,
    EXPLORATION,
    COMBAT,
    BOSS,
    WIN
}

public class MusicControler: MonoBehaviour
{
    public static MusicControler _instance;

    [SerializeField]
    AudioSource audioSource, audioSourceUI;
    [SerializeField]
    List<AudioClip> audioClips;
    [SerializeField]
    AudioClip audioClipUI, audioClipOpen, audioClipClose;

    Coroutine coroutine;

    private void Awake()
    {
        _instance = this;
    }

    public void StartCouroutineFade(float duration, float targetVolume, MusicClip musicClip, float nextVolume = 1)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StartFade(duration, targetVolume, musicClip, nextVolume));
    }

    public IEnumerator StartFade(float duration, float targetVolume, MusicClip musicClip, float nextVolume = 1)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if(musicClip != MusicClip.NONE) {
            audioSource.clip = audioClips[(int)musicClip];
            audioSource.Play();
            coroutine = StartCoroutine(StartFade(duration, nextVolume, MusicClip.NONE));
        }
        yield break;
    }

    public void AudioSelect()
    {
        audioSourceUI.PlayOneShot(audioClipUI);
    }

    public void AudioOpen()
    {
        audioSourceUI.PlayOneShot(audioClipOpen);
    }

    public void AudioClose()
    {
        audioSourceUI.PlayOneShot(audioClipClose);
    }
}
