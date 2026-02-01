using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    public bool isOn;
    public Light2D spotLight;
    public Light2D circleLight;

    public void ToggleTorch()
    {
        if (!GetComponentInParent<PlayerController>().isNightVisionOn)
        {
            if (isOn)
            {
                spotLight.gameObject.SetActive(false);
                circleLight.gameObject.SetActive(false);
            }
            else
            {
                spotLight.gameObject.SetActive(true);
                circleLight.gameObject.SetActive(true);
            }
            isOn = !isOn;
        }   
    }

    public void RotateTorch(Vector2 moveDirection)
    {
        //// Solo rotamos si el personaje se está moviendo
        //if (moveDirection.sqrMagnitude > 0.01f)
        //{
        //    // Atan2 devuelve el ángulo en radianes, lo pasamos a grados
        //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        //    // Aplicamos la rotación en el eje Z. 
        //    // Restamos 90 si tu luz "mira" hacia arriba por defecto en el prefab.
        //    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        //}

        // 1. Obtener la posición del ratón en píxeles (pantalla)
        Vector3 mousePos = Input.mousePosition;

        // 2. Convertir esa posición a coordenadas del mundo (importante en 2D)
        // Usamos Camera.main para referenciar la cámara principal
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // 3. Calcular la dirección: Destino (ratón) - Origen (linterna)
        Vector2 direction = (Vector2)worldMousePos - (Vector2)transform.position;

        // 4. Calcular el ángulo usando Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Aplicar la rotación. 
        // He dejado el "- 90f" por si tu sprite/luz mira hacia arriba por defecto
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotLight.gameObject.SetActive(false);
        circleLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
