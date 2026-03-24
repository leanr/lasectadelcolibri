using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    [Header("Initial Room Limits")]
    public float initialMinX = -13.3f;
    public float initialMaxX = 13.3f;
    public float initialMinY = -7.3f;
    public float initialMaxY = 7.3f;

    [Header("Factory Interior Room Limits")]
    public float factoryMinX = -13.3f;
    public float factoryMaxX = 13.3f;
    public float factoryMinY = -7.3f;
    public float factoryMaxY = 7.3f;

    [Header("Final boss Room Limits")]
    public float bossMinX = -13.3f;
    public float bossMaxX = 13.3f;
    public float bossMinY = -7.3f;
    public float bossMaxY = 7.3f;

    public Volume globalVolume; // Arrastra tu Global Volume aquí
    private LensDistortion _lens;
    private Tween tween;
    public bool isDistorsionOn;

    [HideInInspector]
    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        UpdateCameraLimitsInitial();
        if (globalVolume.profile.TryGet(out _lens))
        {
            _lens.intensity.Override(0f);
            isDistorsionOn = false;
        }
    }

    public void ApplyDistorsion()
    {
        //isDistorsionOn = true;

        //_lens.intensity.value = -0.5f;

        //// 2. Creamos el Tween que va desde el valor actual (-0.5) hasta el otro extremo (0.5)
        //tween = DOTween.To(() => _lens.intensity.value, x => _lens.intensity.value = x, 0.5f, 1f)
        //    .SetLoops(-1, LoopType.Yoyo) // Hace que rebote infinitamente
        //    .SetEase(Ease.InOutCirc);    // Hace que el cambio de dirección sea suave y no brusco

        // 1. Seguridad: Si ya hay un tween, lo matamos antes de empezar otro
        if (tween != null && tween.IsActive()) tween.Kill();

        isDistorsionOn = true;
        _lens.intensity.value = -0.5f;

        tween = DOTween.To(() => _lens.intensity.value, x => _lens.intensity.value = x, 0.5f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCirc)
            .SetLink(gameObject); // <--- IMPORTANTE: Mata el tween si el objeto se destruye
    }

    public void StopDistorsion()
    {
        tween.Kill();
        _lens.intensity.value = 0;
        isDistorsionOn = false;
    }

    public void UpdateCameraLimitsInitial()
    {
        minX = initialMinX;
        maxX = initialMaxX;
        minY = initialMinY;
        maxY = initialMaxY;
    }

    public void UpdateCameraLimitsFactory()
    {
        minX = factoryMinX;
        maxX = factoryMaxX;
        minY = factoryMinY;
        maxY = factoryMaxY;
    }

    public void UpdateCameraLimitsBoss()
    {
        minX = bossMinX;
        maxX = bossMaxX;
        minY = bossMinY;
        maxY = bossMaxY;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 playerPosition = PlayerController.instance.transform.position;
        //transform.position = new Vector3(playerPosition.x, playerPosition.y, -10f);

        if (PlayerController.instance == null) return;

        Vector3 playerPosition = PlayerController.instance.transform.position;

        // Calculamos la posición deseada, pero limitada (Clamped)
        float clampedX = Mathf.Clamp(playerPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(playerPosition.y, minY, maxY);

        // Aplicamos la posición manteniendo el -10 en Z
        transform.position = new Vector3(clampedX, clampedY, -10f);

    }
}
