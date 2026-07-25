using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Çarpan obje bir araba mı (CarAI scripti taşıyor mu) kontrol et
        CarAI car = other.GetComponent<CarAI>();

        if (car != null)
        {
            // Eğer araç hız sınırını aşıyorsa VE biz ona tıklayıp ceza kesmemişsek (Yazısı CEZA değilse)
            if (car.isSpeeding && car.speedText.text != "CEZA!")
            {
                Debug.Log("Uyarı: Hız sınırını aşan araç kaçtı! Can eksiliyor...");

                // GameManager'a bağlan, canı düşür, arayüzü (UI) güncelle ve gerekirse oyunu bitir!
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.TakeDamage();
                }
            }

            // Araba sahnede görevini tamamladı, onu RAM'den tamamen sil
            Destroy(other.gameObject);
        }
    }
}