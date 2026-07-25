using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton deseni: Sahnedeki diğer tüm scriptlerin bu koda kolayca ulaşmasını sağlar
    public static GameManager Instance;

    [Header("Oyuncu İstatistikleri")]
    public int playerHealth = 10;

    [Header("Arayüz (UI) Bağlantıları")]
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        // Singleton bağlantısını kur
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateHealthUI(); // Oyun başlar başlamaz yazıyı güncelle
    }

    /// <summary>
    /// Oyuncu ceza yediğinde canını düşürür ve arayüzü günceller.
    /// </summary>
    public void TakeDamage()
    {
        playerHealth--;
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "CAN: " + playerHealth.ToString();
        }
    }

    private void GameOver()
    {
        Debug.Log("💀 OYUN BİTTİ! Canın sıfırlandı.");
        Time.timeScale = 0f; // Oyunu tamamen durdur
    }
}