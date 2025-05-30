using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public Transform uiContainer;

    [Header("Positioning Settings")]
    public Vector2 topLeftOffset = new Vector2(50f, -50f); // Sol üstten uzaklık
    public Vector2 healthBarSize = new Vector2(250f, 25f); // Health bar boyutu

    private GameObject healthBarObj;
    private PlayerHealth playerHealthScript;

    void Start()
    {
        Invoke("InitializeManager", 0.2f);
    }

    void InitializeManager()
    {
        Debug.Log("PlayerHealthBarManager başlatılıyor...");

        playerHealthScript = GetComponent<PlayerHealth>();
        if (playerHealthScript == null)
        {
            Debug.LogError("PlayerHealth script bulunamadı!");
            return;
        }

        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar prefab atanmamış!");
            return;
        }

        // UI Container yoksa, ana Canvas'ı bul veya oluştur
        if (uiContainer == null)
        {
            uiContainer = FindOrCreateMainCanvas();
        }

        // Önce mevcut health bar var mı kontrol et
        GameObject[] existingBars = GameObject.FindGameObjectsWithTag("PlayerUI");
        if (existingBars.Length > 0)
        {
            Debug.Log("Mevcut PlayerUI health bar bulundu, pozisyon ayarlanıyor.");
            healthBarObj = existingBars[0];
            SetupHealthBarPosition(healthBarObj);

            healthBar existingHealthBar = healthBarObj.GetComponent<healthBar>();
            if (existingHealthBar != null)
            {
                SetupHealthBar(existingHealthBar);
                return;
            }
        }

        // Yeni health bar oluştur
        healthBarObj = Instantiate(healthBarPrefab, uiContainer);
        healthBarObj.tag = "PlayerUI";
        healthBarObj.name = "Player Health Bar (UI)";

        // Pozisyon ayarla
        SetupHealthBarPosition(healthBarObj);

        // Health bar script'ini ayarla
        healthBar healthBarScript = healthBarObj.GetComponent<healthBar>();
        if (healthBarScript != null)
        {
            SetupHealthBar(healthBarScript);
        }
        else
        {
            Debug.LogError("Health Bar script bulunamadı!");
        }
    }

    Transform FindOrCreateMainCanvas()
    {
        // Önce mevcut canvas'ları ara
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                Debug.Log("Mevcut Screen Space Overlay Canvas bulundu: " + canvas.name);
                return canvas.transform;
            }
        }

        // Canvas bulunamadıysa yeni oluştur
        GameObject canvasGO = new GameObject("Main UI Canvas");
        Canvas canvas_new = canvasGO.AddComponent<Canvas>();
        canvas_new.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas_new.sortingOrder = 10; // UI'ın en üstte görünmesi için

        // Canvas Scaler ekle
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // GraphicRaycaster ekle (UI etkileşimi için)
        canvasGO.AddComponent<GraphicRaycaster>();

        Debug.Log("Yeni Main UI Canvas oluşturuldu.");
        return canvasGO.transform;
    }

    void SetupHealthBarPosition(GameObject healthBar)
    {
        RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("Health Bar'da RectTransform bulunamadı!");
            return;
        }

        // Sol üst köşe anchor ayarları
        rectTransform.anchorMin = new Vector2(0f, 1f); // Sol üst
        rectTransform.anchorMax = new Vector2(0f, 1f); // Sol üst
        rectTransform.pivot = new Vector2(0f, 1f); // Sol üst pivot

        // Pozisyon ayarla (sol üstten uzaklık)
        rectTransform.anchoredPosition = topLeftOffset;

        // Boyut ayarla
        rectTransform.sizeDelta = healthBarSize;

        Debug.Log("Health Bar pozisyonu ayarlandı: " + rectTransform.anchoredPosition + ", Boyut: " + rectTransform.sizeDelta);
    }

    void SetupHealthBar(healthBar healthBarScript)
    {
        Debug.Log("Health Bar ayarları yapılıyor...");

        healthBarScript.ownerType = healthBar.OwnerType.Player;

        // Manuel başlatma kullan
        healthBarScript.ManualInitialize(
            playerHealthScript.maxHealth,
            playerHealthScript.currentHealth,
            healthBar.OwnerType.Player
        );

        playerHealthScript.healthBarUI = healthBarScript;

        Debug.Log("Player Health Bar başarıyla oluşturuldu ve ayarlandı!");
    }
}