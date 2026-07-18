using UnityEngine;

/// <summary>
/// Manages the dynamic spawning of vehicles. 
/// Progressively increases difficulty by reducing spawn intervals until Boss Mode is reached.
/// </summary>
public class CarSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Array of vehicle prefabs to be spawned.")]
    public GameObject[] carPrefabs;

    [Tooltip("Array of wall objects where vehicles will spawn from.")]
    public GameObject[] spawnWalls;

    [Header("Difficulty & Time Settings")]
    [Tooltip("Starting time delay (in seconds) between each spawn.")]
    public float initialSpawnInterval = 3f;

    [Tooltip("Minimum time delay between spawns (Maximum difficulty / Boss Mode).")]
    public float bossModeSpawnInterval = 0.5f;

    [Tooltip("How much the spawn interval decreases each time difficulty scales up.")]
    public float difficultyDecrement = 0.2f;

    [Tooltip("Time interval (in seconds) required to increase the game difficulty.")]
    public float difficultyIncreaseTimer = 5f;

    // Internal state variables 
    // Not: Profesyonel C# standartlarında private değişkenler alt tire (_) ile başlar.
    private float _currentSpawnInterval;
    private float _spawnTimer;
    private float _difficultyTimer;
    private bool _isBossMode = false;

    private void Start()
    {
        // Oyunu başlangıç zorluğu ile başlat
        _currentSpawnInterval = initialSpawnInterval;
    }

    private void Update()
    {
        HandleSpawning();
        HandleDifficultyProgression();
    }

    /// <summary>
    /// Handles the countdown and instantiation of vehicles.
    /// </summary>
    private void HandleSpawning()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _currentSpawnInterval)
        {
            SpawnRandomVehicle();
            _spawnTimer = 0f; // Spawn sayacını sıfırla
        }
    }

    /// <summary>
    /// Gradually decreases the spawn interval to increase game difficulty over time.
    /// Clamps at bossModeSpawnInterval.
    /// </summary>
    private void HandleDifficultyProgression()
    {
        // Eğer zaten maksimum zorluktaysak (Boss Mode), hesaplama yapmayı bırakarak performansı koru
        if (_isBossMode) return;

        _difficultyTimer += Time.deltaTime;

        if (_difficultyTimer >= difficultyIncreaseTimer)
        {
            // Zorluğu artır (süreyi kısalt). 
            // Mathf.Max kullanarak sürenin Boss Mode sınırından daha aşağı düşmesini tek satırda engelliyoruz.
            _currentSpawnInterval = Mathf.Max(bossModeSpawnInterval, _currentSpawnInterval - difficultyDecrement);
            _difficultyTimer = 0f; // Zorluk sayacını sıfırla

            // Boss Mode'a ulaşıldı mı kontrolü
            if (_currentSpawnInterval <= bossModeSpawnInterval)
            {
                _isBossMode = true;
                Debug.Log("🔥 BOSS MODE ACTIVATED: Maksimum trafik yoğunluğuna ulaşıldı!");
            }
        }
    }

    /// <summary>
    /// Instantiates a randomly selected car prefab at a randomly selected spawn wall.
    /// </summary>
    private void SpawnRandomVehicle()
    {
        // Array'ler boşsa hata vermemesi için güvenlik kontrolü (Null Exception önlemi)
        if (carPrefabs.Length == 0 || spawnWalls.Length == 0) return;

        int randomCarIndex = Random.Range(0, carPrefabs.Length);
        int randomWallIndex = Random.Range(0, spawnWalls.Length);

        GameObject selectedCar = carPrefabs[randomCarIndex];
        GameObject selectedWall = spawnWalls[randomWallIndex];

        Instantiate(selectedCar, selectedWall.transform.position, selectedWall.transform.rotation);
    }
}