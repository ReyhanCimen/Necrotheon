using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color enemyTargetColor = Color.red;
    public float maxRange = 100f;

    private Camera playerCamera;

    void Start()
    {
        // Kamerayý referans olarak alalým
        if (Camera.main != null)
        {
            playerCamera = Camera.main;
        }
        else
        {
            Debug.LogError("Ana kamera bulunamadý!");
        }

        // Eðer crosshairImage atanmamýþsa otomatik olarak bulalým
        if (crosshairImage == null)
        {
            crosshairImage = GetComponent<Image>();
        }
    }

    void Update()
    {
        if (playerCamera == null || crosshairImage == null) return;

        // Kameradan ileri doðru ýþýn gönderelim
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Iþýn bir nesneye çarparsa
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            // Eðer düþman etiketine sahipse niþangahýn rengini deðiþtirelim
            if (hit.collider.CompareTag("Enemy"))
            {
                crosshairImage.color = enemyTargetColor;
            }
            else
            {
                crosshairImage.color = normalColor;
            }
        }
        else
        {
            // Hiçbir þeye çarpmýyorsa
            crosshairImage.color = normalColor;
        }
    }
}