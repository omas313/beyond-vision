using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioClip _blipSound;
    [SerializeField] AudioClip _enemyAttackSound;
    [SerializeField] AudioClip _playerDeathSound;
    [SerializeField] Range _blipPitchRange;
    [SerializeField] Range _blipVolumeRange;
    AudioSource _audioSource;


    [ContextMenu("play low blip")]
    public void PlayLowBlipSound()
    {
        _audioSource.Stop();
        _audioSource.pitch = _blipPitchRange.min;
        _audioSource.volume = _blipVolumeRange.min;
        _audioSource.loop = true;
        _audioSource.clip = _blipSound;
        _audioSource.Play();
    }

    [ContextMenu("play high blip")]
    public void PlayHighBlipSound()
    {
        _audioSource.Stop();
        _audioSource.pitch = _blipPitchRange.max;
        _audioSource.volume = _blipVolumeRange.max;
        _audioSource.loop = true;
        _audioSource.clip = _blipSound;
        _audioSource.Play();
    }

    public IEnumerator PlayEnemyAttackSound()
    {
        SetupForOneShot();
        _audioSource.PlayOneShot(_enemyAttackSound);
        yield return new WaitForSeconds(_enemyAttackSound.length + 0.1f);
    }

    public IEnumerator PlayPlayerDeathSound()
    {
        SetupForOneShot();
        _audioSource.PlayOneShot(_playerDeathSound);
        yield return new WaitForSeconds(_playerDeathSound.length + 0.5f);
    }

    void SetupForOneShot()
    {
        _audioSource.Stop();
        _audioSource.volume = 0.3f;
        _audioSource.loop = false;
        _audioSource.pitch = 1f;
    }


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    [System.Serializable]
    private class Range
    {
        public float min;
        public float max;

        public float GetLevel(float percent) => min + percent * (max - min);
    }
}

