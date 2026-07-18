using UnityEngine;
using TMPro;

public class CarAI : MonoBehaviour
{
    public float speed;
    public int speedLimit = 90;
    public bool isSpeeding = false;
    public TextMeshProUGUI speedText;

    // Arabaların fiziksel hızını artırmak için bir katsayı çarpanı
    // Bunu Unity içinden (Inspector) istediğin gibi değiştirebilirsin!
    public float movementMultiplier = 3f;

    void Start()
    {
        int randomSpeed = Random.Range(70, 121);
        speed = randomSpeed;

        if (speed > speedLimit)
        {
            isSpeeding = true;
            speedText.color = Color.red;
        }
        else
        {
            speedText.color = Color.green;
        }

        speedText.text = speed.ToString() + " km/h";
    }

    void Update()
    {
        // 1. GÖRSEL HIZ KATSAYISI:
        // Ekranda 78 yazsa bile, arka planda movementMultiplier (şu an 3) ile çarpılıp daha hızlı gidiyor.
        transform.Translate(Vector3.forward * (speed / 10f) * movementMultiplier * Time.deltaTime);

        // 2. YAZILARI KAMERAYA DÖNDÜRME (Billboard Efekti):
        // Yazının açısını, doğrudan ana kameranın açısına eşitliyoruz. 
        // Böylece araba hangi yöne giderse gitsin yazı hep sana doğru bakar.
        if (speedText != null)
        {
            speedText.transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void OnMouseDown()
    {
        if (isSpeeding)
        {
            Debug.Log("Tebrikler, doğru tespit! Puan kazandın.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("HATA! Masum araca ceza kestin. Can eksi 1");
            Destroy(gameObject);
        }
    }
}