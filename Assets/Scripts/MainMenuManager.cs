using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler arası geçiş yapabilmek için şart!

public class MainMenuManager : MonoBehaviour
{
    // Play butonuna basıldığında çalışacak fonksiyon
    public void StartGame()
    {
        // 1 numaralı sahneyi (asıl oyun sahnemizi) yükle
        SceneManager.LoadScene(1);
    }

    // Exit butonuna basıldığında çalışacak fonksiyon
    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        // Oyunun bilgisayar/mobil sürümünde uygulamayı tamamen kapatır
        Application.Quit();
    }
}