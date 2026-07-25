using UnityEngine;
using TMPro;

public class CarAI : MonoBehaviour
{
    public int speed;
    public int speedLimit = 90;
    public bool isSpeeding = false;
    public TextMeshProUGUI speedText;
    public float movementMultiplier = 30f;

    private bool _isProcessed = false;

    void Awake()
    {
        // Araçların iç içe geçmemesi için hızın doğduğu milisaniye belirlenmesi (Awake) şart
        int randomSpeed = Random.Range(70, 121);
        speed = randomSpeed;

        if (speed > speedLimit)
        {
            isSpeeding = true;
            if (speedText != null) speedText.color = Color.red;
        }
        else
        {
            if (speedText != null) speedText.color = Color.green;
        }

        if (speedText != null)
        {
            speedText.text = speed.ToString() + " km/h";
        }
    }

    void Update()
    {
        // Arabayı ileri doğru hareket ettir
        transform.Translate(Vector3.forward * (speed / 10f) * movementMultiplier * Time.deltaTime);

        if (speedText != null)
        {
            speedText.transform.rotation = Camera.main.transform.rotation;
        }
    }

    // Eski ve stabil tıklama mekanizmasına geri döndük
    private void OnMouseDown()
    {
        if (_isProcessed) return;

        _isProcessed = true;

        if (speedText != null)
        {
            speedText.text = "CEZA!";
            speedText.color = Color.blue;
        }

        if (isSpeeding)
        {
            Debug.Log("Doğru tespit! Hızlı araca ceza kestin.");
        }
        else
        {
            Debug.Log("HATA! Masum araca ceza kestin. Can gidiyor!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage();
            }
        }
    }
}