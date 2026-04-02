using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public enum SFX
{
    PaperRustle,
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


    public void Play(SFX sfx)
    {
        if (!soundDict.ContainsKey(sfx))
        {
            Debug.LogWarning($"Sound {sfx} not found in library!");
            return;
        }

        SoundData data = soundDict[sfx];

        AudioSource source = GetSource();

        source.clip = data.clip;
        source.volume = data.volume;
        source.Play();

        StartCoroutine(DisableAfterPlay(source));
    }

    public AudioSource PlayContinously(SFX sfx)
    {
        if (!soundDict.ContainsKey(sfx))
        {
            Debug.LogWarning($"Sound {sfx} not found in library!");
            return null;
        }

        SoundData data = soundDict[sfx];

        AudioSource source = GetSource();

        source.clip = data.clip;
        source.volume = data.volume;
        source.loop = true;
        source.Play();
        return source;
    }

    public void StopContinous(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }
    }

    IEnumerator DisableAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.clip = null;
        source.gameObject.SetActive(false);
    }


}