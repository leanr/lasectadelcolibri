using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossController : MonoBehaviour
{
    public GameObject throwingObject;
    public float throwForce = 10f;
    public float fireRate = 2f;
    public float chargeRate = 11f;
    public float errorMargin = 2f;

    public float yLimitCoord = 3f;
    public Vector2 xCoordRange = new Vector2(-2f, 2f);
    public float chargeAnimationTime = 1f;
    public float returnToOriginalPositionAnimationTime = 2f;

    private float nextFireTime;
    private float nextChargeTime;
    private Transform playerTransform;

    public bool canHit = false;
    public bool charging = false;

    public float bossMaxHealth = 100f;
    [HideInInspector]
    public float currentBossHealth;

    public Slider bossHealthSlider;


    public void ThrowAtPlayer()
    {
        if (!charging)
        {
            if (currentBossHealth > bossMaxHealth / 2f)
            {
                ThrowProjectile();
            }
            else
            {
                fireRate = fireRate / 0.75f;
                ThrowProjectile();
                ThrowProjectile();
            }
            
        }     
    }

    public void ThrowProjectile()
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

    public void UpdateHealthSlider()
    {
        bossHealthSlider.value = currentBossHealth;
    }

    public IEnumerator BossCharge(float previousDelay, float returnToOriginDelay)
    {
        Vector2 originalPosition = new Vector2(transform.position.x, transform.position.y);
        yield return new WaitForSeconds(previousDelay);
        charging = true;
        yield return transform.DOMove(new Vector2(UnityEngine.Random.Range(xCoordRange.x, xCoordRange.y), yLimitCoord), chargeAnimationTime)
            .SetEase(Ease.OutBounce).WaitForCompletion();
        canHit = true;
        yield return new WaitForSeconds(returnToOriginDelay);
        canHit = false;
        yield return transform.DOMove(originalPosition, returnToOriginalPositionAnimationTime).WaitForCompletion();
        charging = false;
    }

    public void TakeDamage(float value)
    {
        currentBossHealth -= value; 
    }

    void Start()
    {
        currentBossHealth = bossMaxHealth;
        bossHealthSlider.maxValue = bossMaxHealth;
        // Buscamos al jugador por su Tag al inicio
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        //StartCoroutine(BossCharge(2f, 2f));
    }

    void Update()
    {
        // Disparar cada cierto tiempo si el jugador existe
        if (Time.time >= nextFireTime && playerTransform != null)
        {
            ThrowAtPlayer();
            nextFireTime = Time.time + fireRate;
        }

        if (Time.time >= nextChargeTime && playerTransform != null)
        {
            StartCoroutine(BossCharge(0f, 3f));
            nextChargeTime = Time.time + chargeRate;
        }

        UpdateHealthSlider();
    }
}