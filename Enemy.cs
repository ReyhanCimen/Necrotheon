using UnityEngine;

public class Enemy : MonoBehaviour
{
    public WaveManager waveManager;
    [Header("Health Settings")]
    public int maxHealth = 100;
    [HideInInspector]
    public int currentHealth;

    public healthBar healthBarUI;
    public bool isEnemyHit = false;

    private EnemySoundManager soundManager;
    private EnemyAnimationManager animationManager;

    void Start()
    {
        currentHealth = maxHealth;

        soundManager = GetComponentInChildren<EnemySoundManager>();
        animationManager = GetComponentInChildren<EnemyAnimationManager>();

        // Health bar başlatmayı geciktir
        Invoke("InitializeHealthBar", 0.1f);
    }

    void InitializeHealthBar()
    {
        if (healthBarUI == null)
        {
            FindHealthBar();
        }

        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Enemy;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = maxHealth;

            if (healthBarUI.healthSlider != null)
            {
                healthBarUI.healthSlider.maxValue = maxHealth;
                healthBarUI.healthSlider.value = maxHealth;
                Debug.Log($"{gameObject.name} - HealthSlider başlatıldı: MaxValue={maxHealth}, Value={maxHealth}");
            }

            if (healthBarUI.easeHealthSlider != null)
            {
                healthBarUI.easeHealthSlider.maxValue = maxHealth;
                healthBarUI.easeHealthSlider.value = maxHealth;
                Debug.Log($"{gameObject.name} - EaseHealthSlider başlatıldı: MaxValue={maxHealth}, Value={maxHealth}");
            }

            Debug.Log($"{gameObject.name} için Health Bar ayarlandı.");
        }
        else
        {
            Debug.LogError($"{gameObject.name} için Health Bar bulunamadı!");
        }
    }

    void FindHealthBar()
    {
        healthBar hb = GetComponentInChildren<healthBar>();
        if (hb != null)
        {
            healthBarUI = hb;
            Debug.Log($"{gameObject.name} - Health Bar çocuk objede bulundu.");
            return;
        }

        GameObject[] bars = GameObject.FindGameObjectsWithTag("EnemyUI");
        foreach (GameObject bar in bars)
        {
            hb = bar.GetComponentInChildren<healthBar>();
            if (hb != null && hb.ownerType == healthBar.OwnerType.Enemy)
            {
                // Bu health bar'ın bu enemy'e yakın olup olmadığını kontrol et
                if (Vector3.Distance(bar.transform.position, transform.position) < 3f)
                {
                    healthBarUI = hb;
                    Debug.Log($"{gameObject.name} - Health Bar EnemyUI tag'i ile bulundu.");
                    break;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // ÖNEMLİ: Can değerini clamp et
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 0'ın altına düşmesin
        isEnemyHit = true;

        Debug.Log($"{gameObject.name} hasar aldı! Hasar: {damage}, Kalan can: {currentHealth}");

        // Health Bar durumunu detaylı kontrol et
        if (healthBarUI != null)
        {
            Debug.Log($"[{gameObject.name}] HealthBar referansı var - Güncellenecek");
            Debug.Log($"[{gameObject.name}] HealthBar önce - Internal Health: {healthBarUI.health}");

            // Önce internal health'i güncelle
            healthBarUI.health = currentHealth;
            Debug.Log($"[{gameObject.name}] HealthBar sonra - Internal Health: {healthBarUI.health}");

            // Ana slider'ı ANINDA güncelle
            if (healthBarUI.healthSlider != null)
            {
                Debug.Log($"[{gameObject.name}] HealthSlider ÖNCE - Value: {healthBarUI.healthSlider.value}, MaxValue: {healthBarUI.healthSlider.maxValue}");

                healthBarUI.healthSlider.value = currentHealth;

                Debug.Log($"[{gameObject.name}] HealthSlider SONRA - Value: {healthBarUI.healthSlider.value}, MaxValue: {healthBarUI.healthSlider.maxValue}");

                // Yüzde hesapla
                float percentage = (healthBarUI.healthSlider.maxValue > 0) ? (currentHealth / healthBarUI.healthSlider.maxValue) * 100f : 0f;
                Debug.Log($"[{gameObject.name}] Health yüzdesi: {percentage:F1}% (Health: {currentHealth}/{healthBarUI.healthSlider.maxValue})");
            }
            else
            {
                Debug.LogError($"[{gameObject.name}] HealthSlider referansı NULL!");
            }

            // Ease slider kontrol
            if (healthBarUI.easeHealthSlider != null)
            {
                Debug.Log($"[{gameObject.name}] EaseHealthSlider - Value: {healthBarUI.easeHealthSlider.value}, MaxValue: {healthBarUI.easeHealthSlider.maxValue}");
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] EaseHealthSlider referansı NULL!");
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name} - Health Bar referansı yok! Tekrar aranıyor...");
            FindHealthBar();
            if (healthBarUI != null)
            {
                InitializeHealthBar();
                // Hasar tekrar uygulanmasın, sadece UI güncelle
                healthBarUI.health = currentHealth;
                if (healthBarUI.healthSlider != null)
                    healthBarUI.healthSlider.value = currentHealth;
            }
        }

        if (soundManager != null)
        {
            soundManager.PlayHitSound();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} öldü!");

        // Can değerini kesinlikle 0 yap
        currentHealth = 0;

        // Health bar'ı sıfırla
        if (healthBarUI != null)
        {
            healthBarUI.health = 0;
            if (healthBarUI.healthSlider != null)
            {
                healthBarUI.healthSlider.value = 0;
                Debug.Log($"{gameObject.name} - Health Bar ölümde sıfırlandı - Value: {healthBarUI.healthSlider.value}");
            }
            if (healthBarUI.easeHealthSlider != null)
            {
                healthBarUI.easeHealthSlider.value = 0;
            }
        }

        if (soundManager != null)
        {
            soundManager.PlayDeathSound();
        }
        if (animationManager != null)
        {
            animationManager.PlayDeadAnimation();
        }
        if (waveManager != null)
            waveManager.OnZombieKilled(gameObject);

        Destroy(gameObject, 1.0f);
    }
}