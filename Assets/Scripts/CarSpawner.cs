using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class CarSpawner : MonoBehaviour
{
    [Header("Stage (Aşama) Ayarları")]
    public int[] carsPerStage = { 5, 10, 20, 30, 40, 50, 60, 75, 90, 120 };
    private int _currentStageIndex = 0;
    private int _carsSpawnedThisStage = 0;

    [Header("Arayüz Bağlantıları")]
    public TextMeshProUGUI stageText;

    [Header("Araç ve Şerit Ayarları")]
    public GameObject[] carPrefabs;
    public GameObject[] spawnWalls;

    [Header("Matematiksel Çarpışma Önleyici")]
    public float roadLength = 80f;
    public float maxPossibleSpeed = 120f;
    [Tooltip("Araçlar arasına eklenecek garanti fiziksel boşluk (saniye)")]
    public float safetyBuffer = 1.5f; // İç içe geçmeye karşı güvenliği artırdık

    private float[] _laneCooldowns;

    // Şeritleri eşit dağıtmak için en son kullanılan şeridi aklında tutar
    private int _lastUsedLaneIndex = -1;

    [Header("Zorluk (Zaman) Ayarları")]
    public float baseSpawnDelay = 2.5f;
    public float minSpawnDelay = 0.8f;

    private float _currentSpawnDelay;
    private float _spawnTimer = 0f;

    private bool _isStageSpawning = true;
    private bool _gameWon = false;

    private void Start()
    {
        _laneCooldowns = new float[spawnWalls.Length];
        StartStage();
    }

    private void Update()
    {
        if (_gameWon) return;

        UpdateLaneCooldowns();

        if (_isStageSpawning)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= _currentSpawnDelay && _carsSpawnedThisStage < carsPerStage[_currentStageIndex])
            {
                TrySpawnCar();
            }

            if (_carsSpawnedThisStage >= carsPerStage[_currentStageIndex])
            {
                _isStageSpawning = false;
            }
        }
        else
        {
            if (FindObjectsByType<CarAI>(FindObjectsSortMode.None).Length == 0)
            {
                NextStage();
            }
        }
    }

    private void UpdateLaneCooldowns()
    {
        for (int i = 0; i < _laneCooldowns.Length; i++)
        {
            if (_laneCooldowns[i] > 0)
            {
                _laneCooldowns[i] -= Time.deltaTime;
            }
        }
    }

    private void TrySpawnCar()
    {
        if (carPrefabs.Length == 0 || spawnWalls.Length == 0) return;

        // DENGELİ ŞERİT SEÇİMİ (Round-Robin Algoritması)
        // Rastgele seçmek yerine, en son çıkılan şeridin bir sonrakine bakar. 
        // Doluysa diğerine geçer. Böylece trafik tüm yola eşit yayılır.
        int selectedLaneIndex = -1;

        for (int i = 1; i <= spawnWalls.Length; i++)
        {
            int checkIndex = (_lastUsedLaneIndex + i) % spawnWalls.Length;

            if (_laneCooldowns[checkIndex] <= 0f)
            {
                selectedLaneIndex = checkIndex;
                break;
            }
        }

        // Eğer tüm şeritlerin süresi kilitliyse, üretimi pas geç (iç içe geçmeyi engeller)
        if (selectedLaneIndex == -1) return;

        int randomCarIndex = Random.Range(0, carPrefabs.Length);
        GameObject selectedCarPrefab = carPrefabs[randomCarIndex];
        GameObject selectedWall = spawnWalls[selectedLaneIndex];

        // Aracı Sahnede Yarat
        GameObject spawnedCar = Instantiate(selectedCarPrefab, selectedWall.transform.position, selectedWall.transform.rotation);

        // Dinamik Matematik Hesaplaması
        CarAI carScript = spawnedCar.GetComponent<CarAI>();
        if (carScript != null)
        {
            float realSpeed = (carScript.speed / 10f) * carScript.movementMultiplier;
            float maxRealSpeed = (maxPossibleSpeed / 10f) * carScript.movementMultiplier;

            float timeToFinish = roadLength / realSpeed;
            float minTimeToFinish = roadLength / maxRealSpeed;

            float dynamicCooldown = (timeToFinish - minTimeToFinish) + safetyBuffer;

            // Şeridi hesaplanan bu süre kadar kilitle
            _laneCooldowns[selectedLaneIndex] = Mathf.Max(safetyBuffer, dynamicCooldown);
        }

        // En son kullandığımız şeridi hafızaya al ki bir dahaki sefere diğerinden başlasın
        _lastUsedLaneIndex = selectedLaneIndex;

        _carsSpawnedThisStage++;
        _spawnTimer = 0f;
    }

    private void NextStage()
    {
        _currentStageIndex++;

        if (stageText != null)
        {
            // Mevcut stage değerini (currentStage) ekrana yazdır
            // Eğer index 0'dan başlıyorsa, oyuncu için (currentStage + 1) olarak yazdırabilirsin
            stageText.text = "STAGE: " + (_currentStageIndex + 1).ToString();
        }

        if (_currentStageIndex >= carsPerStage.Length)
        {
            Debug.Log("🏆 OYUN BİTTİ! 10. Stage Tamamlandı!");
            _gameWon = true;
            return;
        }

        StartStage();
    }

    private void StartStage()
    {
        _carsSpawnedThisStage = 0;
        _isStageSpawning = true;

        float difficultyFactor = (float)_currentStageIndex / (carsPerStage.Length - 1);
        _currentSpawnDelay = Mathf.Lerp(baseSpawnDelay, minSpawnDelay, difficultyFactor);

        Debug.Log($"🏁 STAGE {_currentStageIndex + 1} BAŞLADI! Gelecek araç sayısı: {carsPerStage[_currentStageIndex]}");
    }
}