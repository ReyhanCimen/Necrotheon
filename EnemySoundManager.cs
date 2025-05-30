using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip attackClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("EnemySoundManager: AudioSource component eksik, otomatik eklendi.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
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
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("EnemySoundManager: Ses klibi atanmadı.");
        }
    }
}
