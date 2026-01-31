using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed = 3f;
    private Rigidbody2D rb;
    private Transform target;
   
    
    [Header("Movimiento")]
    public float baseSpeed = 3f;
    public float baseVisionRange = 5f;



    [Header("Aturdimiento")]
    public bool aturdido = false;
    public float tiempoAturdido = 2f;



    [Header("Atención")]
    [Range(0f, 100f)]
    public float attention = 0f;          // Barra de atención
    public float attentionMax = 100f;
    public float incrementPerSecond = 10f; // aumento por segundo
    public float decrementPerSecond = 5f;  // disminución por segundo
    public float maxSpeedBonus = 2f;      // cuánto aumenta la velocidad
    public float maxVisionBonus = 5f;     // cuánto aumenta la visión



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Buscar player por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        
    }

    void FixedUpdate()
    {

        if (aturdido)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        if (target == null) return;

        // Vector hacia el player
        Vector2 direction = (target.position - transform.position);
        float distance = direction.magnitude;


        // Actualizar atención según distancia al player
        if (distance <= baseVisionRange)
        {
            attention += incrementPerSecond * Time.fixedDeltaTime;
            if (attention > attentionMax) attention = attentionMax;
        }
        else
        {
            attention -= decrementPerSecond * Time.fixedDeltaTime;
            if (attention < 0f) attention = 0f;
        }


        // Ajustar velocidad y rango de visión según atención
        float visionRangeActual = baseVisionRange + (attention / attentionMax) * maxVisionBonus;
        float speedActual = baseSpeed + (attention / attentionMax) * maxSpeedBonus;

       


        // Solo perseguir si está dentro del rango de visión
        if (distance <= visionRangeActual)
        {
            direction.Normalize();
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Se queda quieto si el player está fuera del rango
        }


    }

    /*
     * METODO PARA ATURDIR
    public void Aturdir(float duracion)
    {
        if (!aturdido)
            StartCoroutine(AturdidoCoroutine(duracion));
    }
    */

}
