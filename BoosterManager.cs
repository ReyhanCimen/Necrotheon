using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    [Header("Player References")]
    public PlayerHealth playerHealth; // PlayerHealth scriptine referans
    public Gun gun;                 // Gun scriptine referans

    [Header("Ammo Boost Settings")]
    public float fireRateDecreaseMin = 0.02f;   // Ateş hızındaki minimum azalma (daha hızlı ateş)
    public float fireRateDecreaseMax = 0.05f;   // Ateş hızındaki maksimum azalma
    public int magazineIncreaseMin = 3;         // Şarjör kapasitesindeki minimum artış
    public int magazineIncreaseMax = 7;         // Şarjör kapasitesindeki maksimum artış
    public int totalAmmoIncreaseMin = 15;       // Toplam mermi sayısındaki minimum artış
    public int totalAmmoIncreaseMax = 30;       // Toplam mermi sayısındaki maksimum artış

    void Start()
    {
        // Referansları otomatik olarak almaya çalışalım (eğer atanmamışsa)
        if (playerHealth == null)
        {
            // BoosterManager genellikle Player objesine eklenir.
            playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("BoosterManager: PlayerHealth script'i bu obje üzerinde bulunamadı!");
            }
        }

        if (gun == null)
        {
            // Gun scripti genellikle Player objesinin bir child'ında (silah objesi) olur.
            // Veya Player objesinin kendisinde de olabilir.
            gun = GetComponentInChildren<Gun>();
            if (gun == null)
            {
                gun = GetComponent<Gun>();
            }
            if (gun == null)
            {
                // Eğer Player'ın kendisinde veya child'ında değilse, sahnede bulmayı deneyebiliriz.
                // Ancak bu genellikle iyi bir pratik değildir, doğrudan referans daha iyidir.
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    gun = playerObject.GetComponentInChildren<Gun>();
                    if (gun == null) gun = playerObject.GetComponent<Gun>();
                }

                if (gun == null)
                {
                    Debug.LogError("BoosterManager: Gun script'i Player objesinde veya child'larında bulunamadı!");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthBoost"))
        {
            ApplyHealthBoost(other.gameObject);
        }
        else if (other.CompareTag("AmmoBoost"))
        {
            ApplyAmmoBoost(other.gameObject);
        }
    }

    void ApplyHealthBoost(GameObject boosterObject)
    {
        if (playerHealth != null)
        {
            playerHealth.HealToFull(); // PlayerHealth'teki yeni metodu çağır
            Debug.Log("Health Boost alındı! Can tamamen dolduruldu.");
            Destroy(boosterObject); // Boost objesini yok et
        }
        else
        {
            Debug.LogWarning("Health Boost alındı ama PlayerHealth referansı yok!");
        }
    }

    void ApplyAmmoBoost(GameObject boosterObject)
    {
        if (gun != null)
        {
            // Sadece totalAmmo'ya +25 ekle (her zaman)
            gun.totalAmmo += 25;
            Debug.Log($"Ammo Boost: Toplam mermi arttı! Yeni toplam mermi: {gun.totalAmmo} (+25)");

            gun.magazineSize = 10;
            // currentAmmo ve diğer değerleri değiştirme, sadece totalAmmo clamp'le
            gun.totalAmmo = Mathf.Max(0, gun.totalAmmo);

            gun.NotifyAmmoStatsChanged();

            Destroy(boosterObject);
        }
        else
        {
            Debug.LogWarning("Ammo Boost alındı ama Gun referansı yok!");
        }
    }
}