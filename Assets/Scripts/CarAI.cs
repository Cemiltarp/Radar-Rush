using UnityEngine;

public class CarAI : MonoBehaviour
{
    public float speed;
    public int speedLimit = 90;

    private bool isSpeeding = false;
    private float escapeTimer = 5f; // Radardan kaçış süresi

    void Start()
    {
        // Araç doğduğunda 60 ile 130 arası rastgele hız ver
        speed = Random.Range(60f, 130f);

        if (speed > speedLimit)
        {
            isSpeeding = true;
        }
    }

    void Update()
    {
        // Aracı 3D dünyada kendi ön (Forward) yönüne doğru hareket ettir
        transform.Translate(Vector3.forward * (speed / 10f) * Time.deltaTime);

        // Eğer hız sınırını aştıysa 5 saniyelik gerilim sayacını başlat
        if (isSpeeding)
        {
            escapeTimer -= Time.deltaTime;

            if (escapeTimer <= 0)
            {
                Debug.Log("Ceza kaçırıldı! Can eksi 1");
                Destroy(gameObject); // Şimdilik ekrandan siliyoruz, can sistemini sonra bağlayacağız
            }
        }
    }

    // Mobil ekranda dokunmayı veya PC'de mouse tıklamasını algılayan fonksiyon
    private void OnMouseDown()
    {
        if (isSpeeding)
        {
            Debug.Log("Tebrikler, doğru tespit! Ceza kesildi.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("HATA! Kurallara uyan masum araca ceza kestin. Can eksi 1");
            Destroy(gameObject);
        }
    }
}