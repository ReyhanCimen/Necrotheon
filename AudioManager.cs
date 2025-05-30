using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (musicSource != null && background != null)
        {
            musicSource.Stop();
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    /* 
     public AudioClip gunShotClip;
     public AudioClip gunReloadClip;
     public AudioClip outOfAmmoClip;
     public AudioClip deathClip;
     public AudioClip hitClip;
     public AudioClip attackClip;

     [Header("References")]
     public Gun gun;





     //gun
     public void PlayShotSound()
     {
         if (SFXSource != null && gunShotClip != null)
         {
             SFXSource.PlayOneShot(gunShotClip);
         }
     }

     public void PlayReloadSound()
     {
         if (SFXSource != null && gunReloadClip != null)
         {
             SFXSource.PlayOneShot(gunReloadClip);
         }
     }

     public void PlayOutOfAmmoSound()
     {
         if (SFXSource != null && outOfAmmoClip != null)
         {
            SFXSource.PlayOneShot(outOfAmmoClip);
         }
     }

     //enemy
     void Awake()
     {
        SFXSource = GetComponent<AudioSource>();
         if (SFXSource == null)
         {
             Debug.LogWarning("EnemySoundManager: AudioSource component eksik, otomatik eklendi.");
             SFXSource = gameObject.AddComponent<AudioSource>();
         }

        SFXSource.playOnAwake = false;
     }

     public void PlayDeathSound()
     {
         PlayClip(deathClip);
     }

     public void PlayHitSound()
     {
         PlayClip(hitClip);
     }

     public void PlayAttackSound()
     {
         PlayClip(attackClip);
     }

     private void PlayClip(AudioClip clip)
     {
         if (clip != null)
         {
            SFXSource.PlayOneShot(clip);
         }
         else
         {
             Debug.LogWarning("EnemySoundManager: Ses klibi atanmad�.");
         }
     }*/
}
