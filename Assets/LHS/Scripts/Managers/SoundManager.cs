using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Audio { BGM, SFX, Size }

public class SoundManager : MonoBehaviour
{
    GameObject bgmObj;
    AudioSource bgmSource;
    GameObject loopSFX;
    AudioSource addSource;
    List<AudioSource> sfxSources;
    Dictionary<string, AudioClip> audioDic;
    bool isMuted = false;

    private void Awake()
    {
        InitSound();
    }

    public void InitSound()
    {
        sfxSources = new List<AudioSource>();
        audioDic = new Dictionary<string, AudioClip>();
    }

    public void Clear()
    {
        StartCoroutine(ClearRoutine());

        sfxSources.Clear();
        audioDic.Clear();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    IEnumerator ClearRoutine()
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / 1f);
            if (AudioListener.volume <= 0f)
            {
                if (bgmObj != null)
                    GameManager.Resource.Destroy(bgmObj);
                if (loopSFX != null)
                    GameManager.Resource.Destroy(loopSFX);
                isMuted = true;
                yield break;
            }
            yield return null;
        }
    }

    public void FadeInAudio()
    {
        AudioListener.volume = 0f;
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 1f, elapsedTime / 1f);
            if (AudioListener.volume >= 1f)
            {
                isMuted = false;
                yield break;
            }
            yield return null;
        }
    }

    public void PlaySound(AudioClip audioClip, Audio type = Audio.SFX, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        StopCoroutine(FadeInRoutine());
        StopCoroutine(ClearRoutine());

        if (audioClip == null)
            return;

        if (type == Audio.BGM)
        {
            bgmObj = GameManager.Resource.Instantiate<GameObject>("Prefabs/BGM");
            bgmObj.transform.parent = transform;
            bgmSource = bgmObj.GetComponent<AudioSource>();
            if (bgmSource.isPlaying)
                bgmSource.Stop();

            bgmSource.volume = volume;
            bgmSource.pitch = pitch;
            bgmSource.clip = audioClip;
            bgmSource.loop = true;
            bgmObj.name = bgmSource.clip.name;
            bgmSource.Play();
        }
        else
        {
            if (loop)
            {
                loopSFX = GameManager.Resource.Instantiate<GameObject>("Prefabs/SFX");
                addSource = loopSFX.GetComponent<AudioSource>();

                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                loopSFX.name = addSource.clip.name;
                sfxSources.Add(addSource);

                addSource.Play();
            }
            else
            {
                GameObject addObj = GameManager.Resource.Instantiate<GameObject>("Prefabs/SFX", true);

                addObj.transform.parent = transform;
                addSource = addObj.GetComponent<AudioSource>();

                addSource.transform.parent = transform;
                addSource.volume = volume;
                addSource.pitch = pitch;
                addSource.clip = audioClip;
                addSource.loop = loop;
                addObj.name = addSource.clip.name;
                sfxSources.Add(addSource);

                StartCoroutine(SFXPlayRoutine(addObj, audioClip));
            }
        }
    }

    IEnumerator SFXPlayRoutine(GameObject addObj, AudioClip audioClip)
    {
        addSource.PlayOneShot(audioClip);
        yield return new WaitWhile(() => { return addSource.isPlaying; });
        if (addObj != null)
            GameManager.Resource.Destroy(addObj);
        yield break;
    }

    public void PlaySound(string path, Audio type = Audio.SFX, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        PlaySound(audioClip, type, volume, pitch, loop);
    }

    public AudioClip GetOrAddAudioClip(string path, Audio type = Audio.SFX)
    {
        if (path.Contains("Audios/") == false)
            path = $"Audios/{path}";

        AudioClip audioClip = null;

        if (type == Audio.BGM)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (audioDic.TryGetValue(path, out audioClip) == false)
            {
                audioClip = GameManager.Resource.Load<AudioClip>(path);
                audioDic.Add(path, audioClip);
            }
        }

        return audioClip;
    }
}
