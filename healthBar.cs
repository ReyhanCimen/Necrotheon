using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public enum OwnerType { Player, Enemy }
    public OwnerType ownerType = OwnerType.Player;

    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;

    public Color playerHealthColor = Color.green;
    public Color enemyHealthColor = Color.red;

    private bool isInitialized = false;

    void Awake()
    {
        Debug.Log($"[{gameObject.name}] Awake başlıyor - Slider referansları kontrol ediliyor");

        // Ana slider'ı bul
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
            Debug.Log($"[{gameObject.name}] Ana slider bulundu: {(healthSlider != null ? healthSlider.name : "NULL")}");
        }

        // Ease slider'ı bul
        if (easeHealthSlider == null && transform.childCount > 0)
        {
            Slider[] childSliders = GetComponentsInChildren<Slider>();
            Debug.Log($"[{gameObject.name}] Toplam slider sayısı: {childSliders.Length}");

            foreach (Slider s in childSliders)
            {
                Debug.Log($"[{gameObject.name}] Bulunan slider: {s.name}");
                if (s != healthSlider)
                {
                    easeHealthSlider = s;
                    Debug.Log($"[{gameObject.name}] Ease slider atandı: {s.name}");
                    break;
                }
            }
        }

        // Slider'ları manuel ara (eğer hala bulunmadıysa)
        if (healthSlider == null || easeHealthSlider == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Slider referansları eksik! Manuel arama yapılıyor...");
            FindSlidersManually();
        }

        if (health == 0) health = maxHealth;

        Debug.Log($"[{gameObject.name}] Awake tamamlandı - HealthSlider: {(healthSlider != null)}, EaseSlider: {(easeHealthSlider != null)}");
    }

    void FindSlidersManually()
    {
        // Tüm child'larda slider ara
        Slider[] allSliders = GetComponentsInChildren<Slider>(true);

        for (int i = 0; i < allSliders.Length; i++)
        {
            Debug.Log($"[{gameObject.name}] Manuel arama - Slider {i}: {allSliders[i].name}");

            if (healthSlider == null)
            {
                healthSlider = allSliders[i];
                Debug.Log($"[{gameObject.name}] HealthSlider manuel olarak atandı: {healthSlider.name}");
            }
            else if (easeHealthSlider == null && allSliders[i] != healthSlider)
            {
                easeHealthSlider = allSliders[i];
                Debug.Log($"[{gameObject.name}] EaseHealthSlider manuel olarak atandı: {easeHealthSlider.name}");
                break;
            }
        }
    }

    void Start()
    {
        Invoke("InitializeHealthBar", 0.05f);
    }

    void InitializeHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            Debug.Log($"[{gameObject.name}] HealthSlider başlatıldı - MaxValue: {healthSlider.maxValue}, Value: {healthSlider.value}");
        }

        if (easeHealthSlider != null)
        {
            easeHealthSlider.maxValue = maxHealth;
            easeHealthSlider.value = health;
            Debug.Log($"[{gameObject.name}] EaseHealthSlider başlatıldı - MaxValue: {easeHealthSlider.maxValue}, Value: {easeHealthSlider.value}");
        }

        UpdateHealthBarColor();
        isInitialized = true;

        Debug.Log($"[{gameObject.name}] Health bar başlatıldı - Owner: {ownerType}, MaxHealth: {maxHealth}, CurrentHealth: {health}");
    }

    public void UpdateHealthBarColor()
    {
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = (ownerType == OwnerType.Player) ? playerHealthColor : enemyHealthColor;
            }
        }
    }

    void Update()
    {
        if (!isInitialized) return;

        // SADECE EASE SLIDER'I UPDATE ET - NORMAL SLIDER'A DOKUNMA!
        if (easeHealthSlider != null && healthSlider != null)
        {
            if (Mathf.Abs(easeHealthSlider.value - healthSlider.value) > 0.01f)
            {
                // Enemy için daha hızlı lerp (anında görünmesi için)
                float lerpSpeedMultiplier = (ownerType == OwnerType.Enemy) ? 5f : 1f;
                float targetLerpSpeed = lerpSpeed * lerpSpeedMultiplier * Time.deltaTime * 60f;

                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, targetLerpSpeed);
            }
        }
    }

    public void takeDamage(int damage)
    {
        float oldHealth = health;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"[{gameObject.name}] health bar damage: {damage}, Old: {oldHealth}, New: {health}");

        if (healthSlider != null)
        {
            Debug.Log($"[{gameObject.name}] Slider ÖNCE - Value: {healthSlider.value}");

            // DOĞRUDAN HEALTH DEĞERİNİ SET ET
            healthSlider.value = health;

            Debug.Log($"[{gameObject.name}] Slider SONRA - Value: {healthSlider.value}");
            Debug.Log($"[{gameObject.name}] Health yüzdesi: {(health / maxHealth * 100f):F1}%");
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        Debug.Log($"[{gameObject.name}] Health reset edildi: {health}");

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (easeHealthSlider != null)
        {
            easeHealthSlider.maxValue = maxHealth;
            easeHealthSlider.value = maxHealth;
        }

        UpdateHealthBarColor();
    }

    public float GetHealthPercent()
    {
        if (maxHealth == 0) return 0;
        return health / maxHealth;
    }

    public void ManualInitialize(float maxHp, float currentHp, OwnerType owner)
    {
        ownerType = owner;
        maxHealth = maxHp;
        health = currentHp;

        Debug.Log($"[{gameObject.name}] Manuel başlatma: MaxHealth={maxHealth}, Health={health}, Owner={ownerType}");

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }

        if (easeHealthSlider != null)
        {
            easeHealthSlider.maxValue = maxHealth;
            easeHealthSlider.value = health;
        }

        UpdateHealthBarColor();
        isInitialized = true;
    }

    // SETTER METHOD - PlayerHealth'ten çağrılacak
    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth); // ÖNEMLİ: Clamp ekle

        Debug.Log($"[{gameObject.name}] SetHealth çağrıldı - Yeni Health: {health}");
        Debug.Log($"[{gameObject.name}] Slider referansları - HealthSlider: {(healthSlider != null)}, EaseSlider: {(easeHealthSlider != null)}");

        if (healthSlider != null)
        {
            Debug.Log($"[{gameObject.name}] HealthSlider ÖNCE - Value: {healthSlider.value}, MaxValue: {healthSlider.maxValue}");
            healthSlider.value = health; // Health zaten clamp edildi
            Debug.Log($"[{gameObject.name}] HealthSlider SONRA - Value: {healthSlider.value}");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] HealthSlider referansı NULL! Tekrar aranıyor...");
            FindSlidersManually();
            if (healthSlider != null)
            {
                healthSlider.value = health;
                Debug.Log($"[{gameObject.name}] HealthSlider bulundu ve güncellendi: {healthSlider.value}");
            }
        }

        if (easeHealthSlider != null)
        {
            Debug.Log($"[{gameObject.name}] EaseHealthSlider ÖNCE - Value: {easeHealthSlider.value}");
            easeHealthSlider.value = health; // Health zaten clamp edildi
            Debug.Log($"[{gameObject.name}] EaseHealthSlider SONRA - Value: {easeHealthSlider.value}");
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] EaseHealthSlider referansı NULL!");
        }

        Debug.Log($"[{gameObject.name}] SetHealth tamamlandı - Yüzde: {(health / maxHealth * 100f):F1}%");
    }

    // Sadece görsel efektler için - hasar hesaplama yapmaz
    public void PlayDamageEffect(int damageAmount)
    {
        Debug.Log($"[{gameObject.name}] Hasar efekti oynatılıyor: {damageAmount}");

        // Burada hasar efektleri eklenebilir:
        // - Renk değişimi
        // - Titreme efekti  
        // - Ses efekti
        // - Particle efekti

        // Örnek: Kırmızı yanıp sönme efekti
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                StartCoroutine(DamageFlashEffect(fillImage));
            }
        }
    }

    System.Collections.IEnumerator DamageFlashEffect(Image fillImage)
    {
        Color originalColor = fillImage.color;
        fillImage.color = Color.white; // Beyaz flash
        yield return new WaitForSeconds(0.1f);
        fillImage.color = originalColor;
    }

    // Ölüm durumu için özel metod
    public void SetDead()
    {
        health = 0;

        if (healthSlider != null)
        {
            healthSlider.value = 0;
            Debug.Log($"[{gameObject.name}] SetDead çağrıldı - HealthSlider: {healthSlider.value}");
        }

        if (easeHealthSlider != null)
        {
            easeHealthSlider.value = 0;
            Debug.Log($"[{gameObject.name}] SetDead çağrıldı - EaseHealthSlider: {easeHealthSlider.value}");
        }

        // Ölüm rengi efekti (isteğe bağlı)
        if (healthSlider != null && healthSlider.fillRect != null)
        {
            Image fillImage = healthSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = Color.gray; // Gri renk = ölü
            }
        }
    }
}