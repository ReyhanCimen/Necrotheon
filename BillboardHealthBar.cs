using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    private Camera mainCamera;
    private Canvas canvas;

    void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();

        // Canvas optimizasyonu
        if (canvas != null)
        {
            canvas.worldCamera = mainCamera;
            canvas.sortingOrder = 10;
            canvas.pixelPerfect = false; // Performans için
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Optimize edilmiþ billboard rotasyonu
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Y ekseni rotasyonunu engelle (sadece yatay dönsün)

            if (directionToCamera.sqrMagnitude > 0.01f) // sqrMagnitude daha hýzlý
            {
                transform.rotation = Quaternion.LookRotation(-directionToCamera);
            }
        }
    }
}