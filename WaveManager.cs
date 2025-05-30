using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int[] zombiesPerWave = { 10, 15, 20, 25, 50 };
    public Transform[] zombieSpawnPoints;
    public GameObject zombiePrefab;
    public Text waveMessageText; // Legacy UI.Text


    [Header("UI Panels")]
    public GameObject winPanel;



    [Header("References")]
    public BoosterSpawner boosterSpawner;

    private int currentWave = 0;
    private List<GameObject> aliveZombies = new List<GameObject>();
    private bool waveInProgress = false;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private void Update()
    {
        if (waveInProgress && aliveZombies.Count == 0)
        {
            waveInProgress = false;

            if (currentWave < zombiesPerWave.Length)
            {
                StartCoroutine(StartNextWave());
            }
            else
            {
                waveMessageText.text = "All waves completed!";
                if (winPanel != null)
                    winPanel.SetActive(true);
                else
                    Debug.LogWarning("Win Panel atanmadı!");
            }

        }
    }

    IEnumerator StartNextWave()
    {
        waveMessageText.text = $"Wave {currentWave + 1} is about to start!";
        yield return new WaitForSeconds(1f);

        for (int i = 5; i >= 1; i--)
        {
            waveMessageText.text = $"Wave {currentWave + 1} starts in: {i}";
            yield return new WaitForSeconds(1f);
        }

        waveMessageText.text = "FIGHT!";
        yield return new WaitForSeconds(1f);
        waveMessageText.text = "";

        // Zombileri spawnla
        int zombieCount = zombiesPerWave[currentWave];
        for (int i = 0; i < zombieCount; i++)
        {
            Transform spawnPoint = zombieSpawnPoints[Random.Range(0, zombieSpawnPoints.Length)];
            GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            aliveZombies.Add(zombie);

            // Zombie'ye WaveManager referansı ver (eğer Enemy scripti varsa)
            Enemy enemy = zombie.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.waveManager = this;
            }
        }

        // Boostları spawnla
        if (boosterSpawner != null)
            boosterSpawner.SpawnBoosters();

        waveInProgress = true;
        currentWave++;
    }

    public void OnZombieKilled(GameObject zombie)
    {
        if (aliveZombies.Contains(zombie))
        {
            aliveZombies.Remove(zombie);
        }
        // Wave bitiş kontrolü burada da yapılabilir (güvenlik için)
        if (waveInProgress && aliveZombies.Count == 0)
        {
            waveInProgress = false;
            if (currentWave < zombiesPerWave.Length)
            {
                StartCoroutine(StartNextWave());
            }
            else
            {
                waveMessageText.text = "All waves completed!";
                if (winPanel != null)
                    winPanel.SetActive(true);
                else
                    Debug.LogWarning("Win Panel atanmadı!");
            }

        }
    }
}
