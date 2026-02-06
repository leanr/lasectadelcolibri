using UnityEngine;

public class ChainConnector : MonoBehaviour
{
    public Transform anchorPoint; // El poste
    public Transform targetPoint;  // El enemigo
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Importante: El Draw Mode debe ser Tiled
    }

    void Update()
    {
        if (anchorPoint == null || targetPoint == null) return;

        // 1. Mantener el inicio de la cadena en el poste
        transform.position = anchorPoint.position;

        // 2. Calcular dirección y distancia
        Vector3 direction = targetPoint.position - anchorPoint.position;
        float distance = direction.magnitude;

        // 3. Rotación corregida para pivot en "Top"
        // Restamos 90 grados porque tu sprite "cae" hacia abajo (eje Y) 
        // pero Unity calcula el ángulo base hacia la derecha (eje X).
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        // 4. Estirar la cadena
        // Como el pivot es Top, el crecimiento ocurre en el eje Y del tamaño del sprite
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, distance);
    }
}