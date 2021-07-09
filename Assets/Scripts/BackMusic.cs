using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _audioClips;
    private AudioSource _audio;
    private Player _player;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
        SetRandomMusic();
        _player.Died += _audio.Stop;
    }

    private void OnDisable()
    {
        _player.Died -= _audio.Stop;
    }

    private void Update()
    {
        if (!_audio.isPlaying)
            SetRandomMusic();
    }

    private void SetRandomMusic()
    {
        _audio.clip = _audioClips[Random.Range(0, _audioClips.Count - 1)];
        _audio.Play();
    }
}
