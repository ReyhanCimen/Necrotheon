using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Canvas healthBarCanvas;
    private GameObject healthBarObj;
    private Enemy enemyScript;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError($"Enemy script not found on {gameObject.name}");
            return;
        }

        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmamýþ!");
            return;
        }

        // Health Bar oluþturmayý geciktir (Enemy Start()'ýndan sonra)
        Invoke("CreateHealthBar", 0.05f);
    }

    void CreateHealthBar()
    {
        // Health Bar prefab'ýný instantiate et
        healthBarObj = Instantiate(healthBarPrefab, transform);
        healthBarObj.transform.localPosition = offset;
        healthBarObj.tag = "EnemyUI";
        healthBarObj.name = $"{gameObject.name}_HealthBar";

        // Canvas ayarlarý - PERFORMANS ÖNEMLÝ
        healthBarCanvas = healthBarObj.GetComponent<Canvas>();
        if (healthBarCanvas != null)
        {
            healthBarCanvas.renderMode = RenderMode.WorldSpace;
            healthBarCanvas.worldCamera = Camera.main;
            
            // Performans optimizasyonlarý
            healthBarCanvas.sortingOrder = 10;
            healthBarCanvas.pixelPerfect = false; // Performans için kapalý
            
            // Scale ayarlarý
            healthBarObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            
            Debug.Log($"{gameObject.name} - Canvas ayarlarý tamamlandý");
        }

        // Health Bar bileþenlerini bul ve ayarla
        healthBar healthBarScript = healthBarObj.GetComponentInChildren<healthBar>();
        if (healthBarScript != null)
        {
            // Owner tipini düþman olarak ayarla
            healthBarScript.ownerType = healthBar.OwnerType.Enemy;

            // Health Bar deðerlerini ayarla
            healthBarScript.maxHealth = enemyScript.maxHealth;
            healthBarScript.health = enemyScript.maxHealth;

            // Slider deðerlerini ANINDA ayarla
            if (healthBarScript.healthSlider != null)
            {
                healthBarScript.healthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.healthSlider.value = enemyScript.maxHealth;
                
                // Slider ayarlarýný optimize et
                healthBarScript.healthSlider.interactable = false; // Performans için
                Debug.Log($"{gameObject.name} - HealthSlider ayarlandý: {enemyScript.maxHealth}");
            }

            if (healthBarScript.easeHealthSlider != null)
            {
                healthBarScript.easeHealthSlider.maxValue = enemyScript.maxHealth;
                healthBarScript.easeHealthSlider.value = enemyScript.maxHealth;
                
                // Ease slider ayarlarýný optimize et
                healthBarScript.easeHealthSlider.interactable = false; // Performans için
                Debug.Log($"{gameObject.name} - EaseHealthSlider ayarlandý: {enemyScript.maxHealth}");
            }

            // Enemy scriptine Health Bar referansýný ata
            enemyScript.healthBarUI = healthBarScript;

            Debug.Log($"Health Bar initialized for {gameObject.name} with max health: {enemyScript.maxHealth}");
        }
        else
        {
            Debug.LogError("Health Bar script bulunamadý!");
        }

        // Billboard script'ini ekle
        BillboardHealthBar billboard = healthBarObj.GetComponent<BillboardHealthBar>();
        if (billboard == null)
        {
            billboard = healthBarObj.AddComponent<BillboardHealthBar>();
        }
    }

    void Update()
    {
        // Health bar pozisyonunu sürekli güncelle (Billboard script'i de var ama emin olmak için)
        if (healthBarObj != null)
        {
            healthBarObj.transform.position = transform.position + offset;
        }
    }
}