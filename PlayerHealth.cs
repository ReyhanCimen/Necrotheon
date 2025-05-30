using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public healthBar healthBarUI;
    public GameObject losePanel;

    private bool isInitialized = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("PlayerHealth Start - CurrentHealth: " + currentHealth + ", MaxHealth: " + maxHealth);

        Invoke("InitializeHealthSystem", 0.1f);
    }

    void InitializeHealthSystem()
    {
        FindHealthBar();

        if (healthBarUI != null)
        {
            InitializeHealthBar();
            isInitialized = true;
            Debug.Log("Player Health System başarıyla başlatıldı. Current Health: " + currentHealth);
        }
        else
        {
            Debug.LogError("PlayerHealth: Health Bar referansı bulunamadı!");
            Invoke("InitializeHealthSystem", 0.5f);
        }
    }

    void FindHealthBar()
    {
        if (healthBarUI != null) return;

        GameObject[] bars = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (GameObject barGO in bars)
        {
            healthBar hb = barGO.GetComponentInChildren<healthBar>(true);
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar bulundu!");
                return;
            }
        }

        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            healthBar hb = c.GetComponentInChildren<healthBar>(true);
            if (hb != null && hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar canvas içinde bulundu!");
                return;
            }
        }

        healthBar[] allHealthBars = FindObjectsOfType<healthBar>(true);
        foreach (healthBar hb in allHealthBars)
        {
            if (hb.ownerType == healthBar.OwnerType.Player)
            {
                healthBarUI = hb;
                Debug.Log("Player Health Bar sahnede bulundu!");
                return;
            }
        }
    }

    void InitializeHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.ownerType = healthBar.OwnerType.Player;
            healthBarUI.maxHealth = maxHealth;
            healthBarUI.health = currentHealth;

            if (healthBarUI.healthSlider != null)
            {
                healthBarUI.healthSlider.maxValue = maxHealth;
                healthBarUI.healthSlider.value = currentHealth;
            }

            if (healthBarUI.easeHealthSlider != null)
            {
                healthBarUI.easeHealthSlider.maxValue = maxHealth;
                healthBarUI.easeHealthSlider.value = currentHealth;
            }

            healthBarUI.UpdateHealthBarColor();
            Debug.Log("Health Bar başlatıldı - Current: " + currentHealth + ", Max: " + maxHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        // Ölü ise hasar alma
        if (isDead)
        {
            Debug.Log("Player zaten öldü, daha fazla hasar alamaz!");
            return;
        }

        if (!isInitialized)
        {
            Debug.LogWarning("Player health system henüz başlatılmamış!");
            return;
        }

        int oldHealth = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("PLAYER DAMAGED! Amount: " + amount + ", Old Health: " + oldHealth + ", Current health: " + currentHealth);

        // Health Bar'ı güncelle - SADECE SETHEALTH KULLAN!
        if (healthBarUI != null)
        {
            // SADECE health bar'ın internal değerlerini güncelle
            healthBarUI.SetHealth(currentHealth);

            // NOT: takeDamage() ÇAĞIRMA! Çünkü bu çifte hasar yaratıyor!
        }
        else
        {
            Debug.LogError("Health Bar referansı yok!");
        }

        // Ölüm kontrolü
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
        isDead = false;
        Debug.Log("PLAYER HEALED TO FULL! Current health: " + currentHealth);

        if (healthBarUI != null)
        {
            healthBarUI.ResetHealth();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died!");

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Lose Panel referansı atanmadı!");
        }
        Time.timeScale = 0f;
    }


    // Test tuşları
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealToFull();
        }
    }

    // Public getter - EnemyAI'ın kontrol etmesi için
    public bool IsDead()
    {
        return isDead;
    }
}