using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    public GameObject throwingObject;
    public float throwForce = 10f;
    public float fireRate = 2f;
    public float errorMargin = 2f;

    private float nextFireTime;
    private Transform playerTransform;

    void Start()
    {
        // Buscamos al jugador por su Tag al inicio
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Disparar cada cierto tiempo si el jugador existe
        if (Time.time >= nextFireTime && playerTransform != null)
        {
            ThrowAtPlayer();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ThrowAtPlayer()
    {
        // 1. Instanciar el proyectil
        GameObject projectile = Instantiate(throwingObject, transform.position, Quaternion.identity);

        // 2. Calcular el punto de destino con error en X
        // Tomamos la posición del player y le sumamos el error solo al eje X
        float randomXOffset = Random.Range(-errorMargin, errorMargin);
        Vector3 targetPosition = new Vector3(playerTransform.position.x + randomXOffset, playerTransform.position.y, playerTransform.position.z);

        // 3. Calcular dirección hacia ese punto de destino modificado
        Vector2 finalDirection = ((Vector2)targetPosition - (Vector2)transform.position).normalized;

        // 4. Aplicar fuerza al Rigidbody2D
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = finalDirection * throwForce;
        }
    }
}