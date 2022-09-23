using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public AudioClip bgm_GhostNormal;

    private AudioSource bgmSource;


    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();

    }

    public void Play()
    {

        bgmSource.Play();
        Invoke(nameof(PlayGhostNormal), bgmSource.clip.length);
    }

    private void PlayGhostNormal()
    {
        bgmSource.clip = bgm_GhostNormal;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
