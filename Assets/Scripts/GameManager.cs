using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Oyuncu Ayarları")]
    public int health = 10; // Canımız[cite: 1]
    public TextMeshProUGUI healthText; // Can yazısı boşluğu[cite: 1]

    [Header("Oyun Sonu Ekranları")]
    public GameObject winPanel; // Kazanma ekranı[cite: 1]
    public GameObject losePanel; // Kaybetme ekranı[cite: 1]



    void Awake()
    {
        // GameManager'ın tekil kopyasını oluştur[cite: 1]
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1f; // Oyun başladığında zamanın normal aktığından emin ol[cite: 1]
    }

    void Start()
    {
        // 1. GÜVENLİK: Oyun başlarken Can yazısını ekranda doğru başlat
        if (healthText != null)
        {
            healthText.text = "CAN: " + health.ToString();
        }

        // 2. GÜVENLİK: Paneller oyun başında kesinlikle kapalı kalsın
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void TakeDamage()
    {
        health--; // Canı düşür[cite: 1]

        if (healthText != null)
        {
            healthText.text = "CAN: " + health.ToString();
        }

        if (health <= 0) // Can sıfırsa veya altındaysa[cite: 1]
        {
            GameOver(); // Kaybetme fonksiyonunu çağır[cite: 1]
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Oyunu dondur[cite: 1]
        if (losePanel != null) losePanel.SetActive(true); // Kaybetme ekranını aç[cite: 1]
    }

    public void GameWon()
    {
        Time.timeScale = 0f; // Oyunu dondur[cite: 1]
        if (winPanel != null) winPanel.SetActive(true); // Kazanma ekranını aç[cite: 1]
    }
    public void RestartGame()
    {
        // Zamanı tekrar normal hızına al (çünkü oyun bitince 0 yapmıştık)
        Time.timeScale = 1f;

        // Aktif olan sahneyi (şu an oynadığımız bölümü) tamamen baştan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}