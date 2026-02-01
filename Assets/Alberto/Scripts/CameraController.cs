using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    [Header("Map Limits")]
    public float minX = -13.3f;
    public float maxX = 13.3f;
    public float minY = -7.3f;
    public float maxY = 7.3f;

    public Volume globalVolume; // Arrastra tu Global Volume aquí
    private LensDistortion _lens;
    private Tween tween;
    public bool isDistorsionOn;

    void Start()
    {
        if (globalVolume.profile.TryGet(out _lens))
        {
            _lens.intensity.Override(0f);
            isDistorsionOn = false;
        }
    }

    public void ApplyDistorsion()
    {
        isDistorsionOn = true;

        _lens.intensity.value = -0.5f;

        // 2. Creamos el Tween que va desde el valor actual (-0.5) hasta el otro extremo (0.5)
        tween = DOTween.To(() => _lens.intensity.value, x => _lens.intensity.value = x, 0.5f, 1f)
            .SetLoops(-1, LoopType.Yoyo) // Hace que rebote infinitamente
            .SetEase(Ease.InOutCirc);    // Hace que el cambio de dirección sea suave y no brusco
    }

    public void StopDistorsion()
    {
        tween.Kill();
        _lens.intensity.value = 0;
        isDistorsionOn = false;
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
