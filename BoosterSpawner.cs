using System.Collections.Generic;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    [Header("Booster Prefabs")]
    public GameObject healthBoostPrefab;
    public GameObject ammoBoostPrefab;

    [Header("Spawn Points")]
    public Transform[] ammoBoostSpawnPoints;
    public Transform[] healthBoostSpawnPoints;

    // Internal state
    private List<Transform> availableSpawnPoints = new List<Transform>();

    private void Start()
    {
        // Başta tüm spawn noktalarını müsait olarak işaretle
        availableSpawnPoints.AddRange(ammoBoostSpawnPoints);
        availableSpawnPoints.AddRange(healthBoostSpawnPoints);
    }

    public void SpawnBoosters()
    {
        // Ammo boostları sabit yerlerde spawnla
        foreach (Transform spawnPoint in ammoBoostSpawnPoints)
        {
            if (ammoBoostPrefab != null && spawnPoint != null)
                Instantiate(ammoBoostPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // Health boostları sabit yerlerde spawnla
        foreach (Transform spawnPoint in healthBoostSpawnPoints)
        {
            if (healthBoostPrefab != null && spawnPoint != null)
                Instantiate(healthBoostPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
