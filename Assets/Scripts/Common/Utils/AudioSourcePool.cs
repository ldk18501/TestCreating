using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

//Kevin.Zhang, 1/23/2017
/// <summary>
/// a wrapper of LeanPool to store audio source
/// </summary>
public class AudioSourcePool : DoozyUI.Singleton<AudioSourcePool>
{
    private GameObject _mPrefab;

    void Awake()
    {
        _mPrefab = new GameObject("AudioSource");
        AudioSource aSource = _mPrefab.AddComponent<AudioSource>();
        aSource.playOnAwake = false;
        _mPrefab.transform.SetParent(this.transform);
        //_mPrefab.hideFlags = HideFlags.HideInHierarchy;
        _mPrefab.SetActive(false);
    }

    public AudioSource Spawn(AudioClip clip, Vector3 position, Transform parent = null)
    {
        AudioSource aSource = LeanPool.Spawn(_mPrefab, position, Quaternion.identity, parent).GetComponent<AudioSource>();
        aSource.transform.parent = this.transform;
        aSource.gameObject.name = "Audio - " + clip.name;
        aSource.clip = clip;
        if (aSource.isPlaying)
        {
            aSource.Stop();
            aSource.time = 0f;
        }

        return aSource;
    }

    public void Free(AudioSource audioSource, float delay = 0f)
    {
        if (audioSource != null)
            LeanPool.Despawn(audioSource.gameObject, delay);
    }
}
