using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalBossController : MonoBehaviour
{
    // throw mechanic variables
    public GameObject throwingObject;
    public float throwForce = 10f;
    public float fireRate = 2f; // we can try to match it with the animation time and then speed it up
    public float errorMargin = 2f;
    private float nextFireTime;
    public bool isThrowing = false;


    // charge mechanic variables
    public float chargeRate = 11f;
    public float yLimitCoord = 0.67f;
    public Vector2 xCoordRange = new Vector2(-3f, 5f);
    public float chargeAnimationTime = 1.5f; // has to match with the animator's animation time - charge
    public float returnToOriginalPositionAnimationTime = 2f; // has to match with the animator's animation time - walk
    private float nextChargeTime;
    public bool isCharging = false;

    // other boss mechanic variables
    public float bossMaxHealth = 100f;
    [HideInInspector]
    public float currentBossHealth;
    public Slider bossHealthSlider;
    private Transform playerTransform;
    public bool canHit = false;
    [HideInInspector]
    public Animator bossAnimator;

    // Generator logic
    public Transform generator1;
    public Transform generator2;
    public bool isThrowingGenerator;
    public bool throwGen1First;
    public int destroyedGenerators;
    public float explosionDelay = 0.5f;

    public void StartThrowAtPlayerAnimation()
    {
        bossAnimator.SetTrigger("throw");
        isThrowing = true;
    }

    // Called from the animator
    public void ThrowRock()
    {
        if (isThrowingGenerator)
        {
            StartCoroutine(ThrowAtGenerator());
        }
        else
        {
            ThrowAtPlayer();
        }
        isThrowingGenerator = false;
    }

    
    public void ThrowAtPlayer()
    {
        if (currentBossHealth > bossMaxHealth / 2f)
        {
            InstantiateAndThrowProjectile(playerTransform.position, errorMargin);
        }

        else
        {
            fireRate = fireRate / 0.75f;
            InstantiateAndThrowProjectile(playerTransform.position, errorMargin);
            InstantiateAndThrowProjectile(playerTransform.position, errorMargin);
        }
    }

    public IEnumerator ThrowAtGenerator()
    {
        GameObject rock = null;
        Animator targetGeneratorAnimator = null;
        Transform targetGenerator = null;

        if (destroyedGenerators == 0)
        {
            if (throwGen1First)
            {
                targetGenerator = generator1;
            }
            else
            {
                targetGenerator = generator2;
            }
        }
        else if (destroyedGenerators == 1)
        {
            if (generator1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                targetGenerator = generator1;
            }
            else
            {
                targetGenerator = generator2;
            }
        }

        rock = InstantiateAndThrowProjectile(targetGenerator.position, 0f);
        targetGeneratorAnimator = targetGenerator.GetComponent<Animator>();
        destroyedGenerators++;

        // Activate poison Gas
        targetGenerator.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(explosionDelay);

        if (rock != null)
        {
            Destroy(rock);
        }

        if (targetGeneratorAnimator != null)
        {
            targetGeneratorAnimator.SetTrigger("boom");
        }


        
    }

    public GameObject InstantiateAndThrowProjectile(Vector3 target, float errorMarginParam)
    {
        // 1. Instanciar el proyectil
        GameObject projectile = Instantiate(throwingObject, transform.position, Quaternion.identity);

        if (!isThrowingGenerator)
        {
            projectile.GetComponent<ThrowingObject>().IgnoreGeneratorCollision(generator1.GetComponent<BoxCollider2D>(), generator2.GetComponent<BoxCollider2D>());
        }

        // 2. Calcular el punto de destino con error en X
        // Tomamos la posición del player y le sumamos el error solo al eje X
        float randomXOffset = Random.Range(-errorMarginParam, errorMarginParam);
        Vector3 targetPosition = new Vector3(target.x + randomXOffset, target.y, target.z);

        // 3. Calcular dirección hacia ese punto de destino modificado
        Vector2 finalDirection = ((Vector2)targetPosition - (Vector2)transform.position).normalized;

        // 4. Aplicar fuerza al Rigidbody2D
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = finalDirection * throwForce;
        }

        return projectile;
    }

    // Called from the animator
    public void StopThrowing()
    {
        isThrowing = false;
    }

    public void UpdateHealthSlider()
    {
        bossHealthSlider.value = currentBossHealth;
    }

    public IEnumerator BossCharge(float previousDelay, float returnToOriginDelay)
    {
        Vector2 originalPosition = new Vector2(transform.position.x, transform.position.y);
        yield return new WaitForSeconds(previousDelay);
        bossAnimator.SetTrigger("charge");
        isCharging = true;
        yield return transform.DOMove(new Vector2(UnityEngine.Random.Range(xCoordRange.x, xCoordRange.y), yLimitCoord), chargeAnimationTime)
            .SetEase(Ease.OutBounce).WaitForCompletion();
        canHit = true;
        yield return new WaitForSeconds(returnToOriginDelay);
        canHit = false;
        yield return transform.DOMove(originalPosition, returnToOriginalPositionAnimationTime).WaitForCompletion();
        isCharging = false;
    }

    public void TakeDamage(float value)
    {
        currentBossHealth -= value;
        isThrowingGenerator = true;
    }

    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        isThrowing = false;
        isCharging = false;
        isThrowingGenerator = false;
        currentBossHealth = bossMaxHealth;
        bossHealthSlider.maxValue = bossMaxHealth;
        // Buscamos al jugador por su Tag al inicio
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        destroyedGenerators = 0;
        throwGen1First = Random.value > 0.5f;
    }

    void Update()
    {
        // Disparar cada cierto tiempo si el jugador existe
        if (Time.time >= nextFireTime && playerTransform != null && !isCharging)
        {
            StartThrowAtPlayerAnimation();
            nextFireTime = Time.time + fireRate;
        }

        // If the boss is not shooting, it charges towards the player if the conditions are met
        if (Time.time >= nextChargeTime && playerTransform != null && !isThrowing && !isThrowingGenerator)
        {
            StartCoroutine(BossCharge(0f, 3f));
            nextChargeTime = Time.time + chargeRate;
        }

        UpdateHealthSlider();
    }
}