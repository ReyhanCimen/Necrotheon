using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public TMP_Text ammoText; // Normal Text yerine TMP_Text kullan�n

    [Header("Gun Reference")]
    public Gun gunScript; // Silah script'i referans�

    private void Start()
    {
        // E�er referans atanmam��sa, kendini referans olarak ata
        if (ammoText == null)
        {
            ammoText = GetComponent<TMP_Text>();
            Debug.Log("AmmoText auto-assigned: " + (ammoText != null));
        }

        // Gun script'ini bul
        if (gunScript == null)
        {
            gunScript = FindObjectOfType<Gun>();
            Debug.Log("Gun script found: " + (gunScript != null));
        }

        // Ba�lang��ta mermi say�s�n� g�ncelle
        UpdateAmmoText();
    }

    private void Update()
    {
        // Her frame mermi say�s�n� g�ncelle
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (gunScript != null && ammoText != null)
        {
            ammoText.text = gunScript.currentAmmo + "/" + gunScript.totalAmmo;
            // Debug.Log("Ammo text updated: " + ammoText.text);
        }
    }


    private void OnDestroy()
    {
        // Event aboneli�ini kald�r (haf�za s�z�nt�s�n� �nlemek i�in)
        if (gunScript != null)
        {
            gunScript.onAmmoChanged -= UpdateAmmoUI;
        }
    }

    // Event handler metodu
    void UpdateAmmoUI(int currentAmmo, int totalAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/" + totalAmmo;
        }
    }
}