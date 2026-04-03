using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public enum SFX
{
    Bubbles,
    UnderwaterExplosion,
    TrashPickup,
    PredatorMusic,
    PredatorAttack,
    TakingDamage,
    Healing,
    SavePointGot,
    InToxicWater,
    Swim,
    ClearingReefs,
    EatFish,
    DeepSeaAmbience
}
[System.Serializable]
public class SoundData
{
    public SFX sfx;
    public AudioClip clip;
    [UnityEngine.Range(0f,1f)] public float volume = 1f;
}


public class AudioManager : Singleton<AudioManager>
{

    [Header("SFX Library")]
    [SerializeField] private List<SoundData> sounds;

    private Dictionary<SFX, SoundData> soundDict;

    private List<AudioSource> pool = new List<AudioSource>();
    private Dictionary<AudioSource, Coroutine> activeFades = new Dictionary<AudioSource, Coroutine>();
    private Dictionary<AudioSource, Coroutine> activeAudio = new Dictionary<AudioSource, Coroutine>();

    protected override void Awake()
    {
        base.Awake();

        soundDict = new Dictionary<SFX, SoundData>();

        foreach (var s in sounds)
            soundDict[s.sfx] = s;
    }

    AudioSource GetSource()
    {
        foreach (var src in pool)
        {
            if (!src.gameObject.activeSelf)
            {
                src.gameObject.SetActive(true);
                return src;
            }
        }

        GameObject obj = new GameObject("AudioSource");
        obj.transform.parent = transform;

        AudioSource source = obj.AddComponent<AudioSource>();
        // source.outputAudioMixerGroup = AudioManager.instance.mixer.FindMatchingGroups("Master")[0];
        pool.Add(source);

        return source;
    }

    private void Reset()
    {
        //make list of all sound types in enum
        sounds = new List<SoundData>();
        foreach (SFX sfx in System.Enum.GetValues(typeof(SFX)))
        {
            sounds.Add(new SoundData { sfx = sfx });
        }
        
    }


    public void PlayOneShot(SFX sfx, Transform transform = null)
    {
        if (!soundDict.ContainsKey(sfx))
        {
            Debug.LogWarning($"Sound {sfx} not found in library!");
            return;
        }

        SoundData data = soundDict[sfx];

        AudioSource source = GetSource();
        if (transform != null)
        {
            source.transform.position = transform.position;
            source.transform.parent = transform;
            source.spatialBlend = 1;
        }
        else
        {
            source.spatialBlend = 0;
        }
        source.clip = data.clip;
        source.volume = data.volume;
        source.loop = false;
        source.Play();
        

        StartCoroutine(DisableAfterPlay(source));
    }
    public void PlayOneShotFree(SFX sfx, Transform transform = null)
    {
        if (!soundDict.ContainsKey(sfx))
        {
            Debug.LogWarning($"Sound {sfx} not found in library!");
            return;
        }

        SoundData data = soundDict[sfx];

        AudioSource source = GetSource();
        if (transform != null)
        {
            source.transform.position = transform.position;
            source.spatialBlend = 1;
        }
        else
        {
            source.spatialBlend = 0;
        }
        source.clip = data.clip;
        source.volume = data.volume;
        source.loop = false;
        source.Play();
        

        StartCoroutine(DisableAfterPlay(source));
    }

    public AudioSource PlayContinously(SFX sfx, Transform transform = null)
    {
        if (!soundDict.ContainsKey(sfx))
        {
            Debug.LogWarning($"Sound {sfx} not found in library!");
            return null;
        }

        SoundData data = soundDict[sfx];

        AudioSource source = GetSource();
        if (transform != null)
        {
            source.transform.position = transform.position;
            source.transform.parent = transform;
            source.spatialBlend = 1;
        }
        else
        {
            source.spatialBlend = 0;
        }
        source.clip = data.clip;
        // source.volume = data.volume;
        Coroutine coroutine = StartCoroutine(LerpVolume(source, data.volume, 10));
        activeFades[source] = coroutine;

        source.loop = true;
        source.Play();
        
        return source;
    }

    IEnumerator LerpVolume(AudioSource source, float volume, float lerpStrength = 0.1f)
    {
        source.volume = 0f;
        while (source.volume < volume - 0.1f)
        {
            if (!source.gameObject.activeInHierarchy) break;
            source.volume = Mathf.Lerp(source.volume, volume, lerpStrength * Time.deltaTime);
            yield return null;
        }
        source.volume = volume;
    }
    IEnumerator LerpVolumeAndStop(AudioSource source, float lerpStrength = 0.1f)
    {
        while (source.volume > 0.1f)
        {
            if (!source.gameObject.activeInHierarchy) break;
            source.volume = Mathf.Lerp(source.volume, 0, lerpStrength * Time.deltaTime);
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        source.transform.parent = transform;
        activeAudio.Remove(source);
    }

    public void StopContinous(AudioSource source)
    {
        if (source != null)
        {
            StopRoutine(source);
            Coroutine coroutine = StartCoroutine(LerpVolumeAndStop(source, 10));
            activeFades[source] = coroutine;

            // source.Stop();
            // source.clip = null;
            // source.gameObject.SetActive(false);
        }
    }
    void StopRoutine(AudioSource source)
    {
        if (source == null) return;

        if (activeFades.TryGetValue(source, out Coroutine c))
        {
            try
            {
                StopCoroutine(c);
            }
            catch
            {}
            activeAudio.Remove(source);
        }

        // source.transform.parent = transform;
    }

    IEnumerator DisableAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
        source.gameObject.SetActive(false);
        source.transform.parent = transform;
    }

    public AudioSource CrossFade(AudioSource fromSource, SFX toSFX, float duration = 1f)
    {
        if (!soundDict.ContainsKey(toSFX))
        {
            Debug.LogWarning($"Sound {toSFX} not found in library!");
            return null;
        }

        SoundData data = soundDict[toSFX];

        AudioSource toSource = GetSource();
        toSource.clip = data.clip;
        toSource.volume = 0f;
        toSource.loop = true;
        toSource.Play();

        StopFade(fromSource);
        StopFade(toSource);

        Coroutine fade = StartCoroutine(CrossFadeRoutine(fromSource, toSource, data.volume, duration));

        // Track both sources (important!)
        if (fromSource != null) activeFades[fromSource] = fade;
        activeFades[toSource] = fade;

        return toSource;
    }
    void StopFade(AudioSource source)
    {
        if (source == null) return;

        if (activeFades.TryGetValue(source, out Coroutine c))
        {
            try
            {
                StopCoroutine(c);
            }
            catch
            {}
            activeFades.Remove(source);
        }

        // source.transform.parent = transform;
    }

    IEnumerator CrossFadeRoutine(AudioSource from, AudioSource to, float targetVolume, float duration)
    {
        float time = 0f;

        float startFromVol = from != null ? from.volume : 0f;

        while (time < duration)
        {
            if ((from != null && !from.gameObject.activeInHierarchy) ||
                (to != null && !to.gameObject.activeInHierarchy))
                yield break;

            time += Time.deltaTime;
            float t = time / duration;

            if (to != null)
                to.volume = Mathf.Lerp(0f, targetVolume, t);

            if (from != null)
                from.volume = Mathf.Lerp(startFromVol, 0f, t);

            yield return null;
        }

        // Finalize
        if (to != null)
        {
            to.volume = targetVolume;
        }

        if (from != null)
        {
            from.Stop();
            from.clip = null;
            from.gameObject.SetActive(false);
            from.transform.parent = transform;
        }

        if (from != null) activeFades.Remove(from);
        if (to != null) activeFades.Remove(to);
    }
}