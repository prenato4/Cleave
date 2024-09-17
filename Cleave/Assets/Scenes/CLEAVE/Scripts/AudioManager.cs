using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private static AudioManager instance;
    
    public AudioSource MusicSouce, sfxsouce;
     public AudioClip Coletavel, Jump;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void OnEnable()
    {
        AudioObserver.PlayMusicEvent += TocarMusica;
        AudioObserver.StopMusicEvent += PararMusica;
        AudioObserver.PlaySFXEvent += TocarEfeitoSonoro;
    }

    private void OnDisable()
    {
        AudioObserver.PlayMusicEvent -= TocarMusica;
        AudioObserver.StopMusicEvent -= PararMusica;
        AudioObserver.PlaySFXEvent -= TocarEfeitoSonoro;
    }


    void TocarEfeitoSonoro(string NomedoClip)
    {
        switch (NomedoClip)
        {
            case "Jump":
                sfxsouce.PlayOneShot(Jump);
                break;
            case "coletavel":
                sfxsouce.PlayOneShot(Coletavel);
                break;
            default:
                Debug.Log($"Efeito Sonoro: {NomedoClip} Não Encontrado");
                break;
        }
    }

    void TocarMusica()
    {
        MusicSouce.Play();
    }
    
    void PararMusica()
    {
        MusicSouce.Stop();
    }
}
