using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public int magazineSize = 10;
    public int totalAmmo = 50;
    public float fireRate = 0.2f; // BoosterManager tarafından değiştirilebilmesi için public kalmalı
    public float range = 100f;

    [Header("References")]
    public Transform muzzlePoint; // Ucu
    public Camera playerCamera;
    [Header("External References")]
    public Weapon2DController weapon2DController;
    public WeaponSoundManager weaponSoundManager;


    // Mermi değişikliği için event
    public delegate void AmmoChangedHandler(int currentAmmo, int totalAmmo);
    public event AmmoChangedHandler onAmmoChanged;

    // Public değişkene çevirdik (UI erişimi için)
    public int currentAmmo { get; set; }
    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = magazineSize;
        NotifyAmmoStatsChanged(); // Başlangıçta UI'ı güncellemek için
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
            else
            {
                Debug.Log("Şarjör boş! R ile doldur.");

                if (weaponSoundManager != null)
                    weaponSoundManager.PlayOutOfAmmoSound();
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("Ateş! Kalan mermi: " + currentAmmo);

        NotifyAmmoStatsChanged(); // Ateş ettikten sonra UI'ı güncelle

        // Sesi çal
        if (weaponSoundManager != null)
            weaponSoundManager.PlayShotSound();

        // Animasyonu başlat
        if (weapon2DController != null)
            weapon2DController.PlayShotAnimation();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Vurulan nesne: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(25); // Örnek hasar değeri
                }
            }
        }
    }


    void Reload()
    {
        if (currentAmmo == magazineSize || totalAmmo <= 0)
        {
            Debug.Log("Şarjör dolu ya da mermi yok.");
            return;
        }

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, totalAmmo);

        totalAmmo -= ammoToReload;
        currentAmmo += ammoToReload;

        Debug.Log("Şarjör değiştirildi. Kalan toplam mermi: " + totalAmmo);

        NotifyAmmoStatsChanged(); // Şarjör değiştirdikten sonra UI'ı güncelle

        if (weaponSoundManager != null)
            weaponSoundManager.PlayReloadSound();
    }

    // YENİ METOD: Dışarıdan çağrıldığında onAmmoChanged olayını tetikler
    // ve UI'ın güncellenmesini sağlar.
    public void NotifyAmmoStatsChanged()
    {
        if (onAmmoChanged != null)
        {
            onAmmoChanged(currentAmmo, totalAmmo); // totalAmmo yerine magazineSize'ı da event'e ekleyebilirsiniz
                                                   // eğer AmmoDisplay'in bu bilgiye event üzerinden ihtiyacı varsa.
                                                   // Mevcut AmmoDisplay Update'te magazineSize'ı zaten alıyor.
            Debug.Log($"Gun: NotifyAmmoStatsChanged çağrıldı. Current: {currentAmmo}, Total: {totalAmmo}, MagSize: {magazineSize}");
        }
    }

    // BoosterManager'ın şarjör kapasitesini artırdıktan sonra
    // mevcut mermiyi de doldurmak istersek kullanabileceğimiz bir metod.
    // Şimdilik BoosterManager sadece magazineSize'ı artırıyor.
    public void AddAmmoToMagazine(int amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, magazineSize);
        NotifyAmmoStatsChanged();
    }
}