using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    public bool isOn;
    private Light2D spotLight;

    public void ToggleTorch()
    {
        if (isOn)
        {
            spotLight.gameObject.SetActive(false);
        }
        else
        {
            spotLight.gameObject.SetActive(true);
        }
        isOn = !isOn;
    }

    public void RotateTorch(Vector2 moveDirection)
    {
        // Solo rotamos si el personaje se está moviendo
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            // Atan2 devuelve el ángulo en radianes, lo pasamos a grados
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Aplicamos la rotación en el eje Z. 
            // Restamos 90 si tu luz "mira" hacia arriba por defecto en el prefab.
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotLight = GetComponentInChildren<Light2D>();
        spotLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
