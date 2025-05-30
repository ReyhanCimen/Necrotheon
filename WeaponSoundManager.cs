using UnityEngine;

public class WeaponSoundManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunShotClip;
    public AudioClip gunReloadClip;
    public AudioClip outOfAmmoClip;

    [Header("References")]
    public Gun gun; // Gun scriptine referans

    // void Update()
    // {
    //     if (Input.GetButtonDown("Fire1"))
    //     {
    //         if (gun != null && gun.currentAmmo <= 0)
    //         {
    //             PlayOutOfAmmoSound();
    //         }
    //         else
    //         {
    //             PlayShotSound();
    //         }
    //     }

    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         PlayReloadSound();
    //     }
    // }

    public void PlayShotSound()
    {
        if (audioSource != null && gunShotClip != null)
        {
            audioSource.PlayOneShot(gunShotClip);
        }
    }

    public void PlayReloadSound()
    {
        if (audioSource != null && gunReloadClip != null)
        {
            audioSource.PlayOneShot(gunReloadClip);
        }
    }

    public void PlayOutOfAmmoSound()
    {
        if (audioSource != null && outOfAmmoClip != null)
        {
            audioSource.PlayOneShot(outOfAmmoClip);
        }
    }
}
