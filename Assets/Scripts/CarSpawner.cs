using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Doğma Noktaları (Duvarlar)")]
    public Transform[] spawnWalls;

    [Header("Araç Modelleri")]
    public GameObject[] carPrefabs;

    private float spawnInterval = 3f; // Başlangıçta 3 saniyede bir araç
    private float timer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        // Her 10 saniyede bir araçların çıkış hızını artır (Zorluk seviyesi)
        if (difficultyTimer >= 10f)
        {
            if (spawnInterval > 0.8f) // Ekranın tamamen arabayla dolmasını engellemek için sınır
            {
                spawnInterval -= 0.2f;
            }
            difficultyTimer = 0f;
        }

        // Kronometre dolduğunda yeni araç yarat
        if (timer >= spawnInterval)
        {
            SpawnCar();
            timer = 0f;
        }
    }

    void SpawnCar()
    {
        // Rastgele bir duvar ve rastgele bir araç modeli seç
        int randomWall = Random.Range(0, spawnWalls.Length);
        int randomCar = Random.Range(0, carPrefabs.Length);

        // Seçilen aracı, seçilen duvarın konumu ve açısıyla sahneye yerleştir (Instantiate)
        Instantiate(carPrefabs[randomCar], spawnWalls[randomWall].position, spawnWalls[randomWall].rotation);
    }
}