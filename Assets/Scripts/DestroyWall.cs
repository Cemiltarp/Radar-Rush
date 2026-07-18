using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Çarpan objenin üzerinde CarAI kodu var mı diye kontrol et
        CarAI car = other.GetComponent<CarAI>();

        if (car != null)
        {
            // Eğer araba hızlıysa ve oyuncu onu tıklamadan duvara kadar kaçmayı başardıysa
            if (car.isSpeeding)
            {
                Debug.Log("Hızlı araç kaçtı! Can eksi 1");
            }
        }

        // Hızlı da olsa yavaş da olsa ekrandan çıkan arabayı oyundan sil
        Destroy(other.gameObject);
    }
}