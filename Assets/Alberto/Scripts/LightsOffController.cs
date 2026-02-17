using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class LightsOffController : MonoBehaviour
{
    public Light2D globalLight;
    public float dimmedIntensity = 0.002f; // La intensidad de ESTE trigger específico
    public float defaultIntensity = 0.4f;
    public float fadeDuration = 3f;

    // Lista compartida por todos los controladores para saber qué intensidades están activas
    private static List<float> activeIntensities = new List<float>();
    private static Tween intensityTween;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Añadimos la intensidad de este trigger a la lista global
            activeIntensities.Add(dimmedIntensity);
            UpdateLightIntensity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Eliminamos esta intensidad de la lista
            activeIntensities.Remove(dimmedIntensity);
            UpdateLightIntensity();
        }
    }

    private void UpdateLightIntensity()
    {
        intensityTween?.Kill();

        float targetValue;

        if (activeIntensities.Count > 0)
        {
            // PRIORIDAD: Buscamos el valor más bajo (mínimo) entre todos los triggers activos
            targetValue = activeIntensities.Min();
        }
        else
        {
            // Si no hay nadie, volvemos al valor por defecto
            targetValue = defaultIntensity;
        }

        intensityTween = DOTween.To(() => globalLight.intensity,
                                    x => globalLight.intensity = x,
                                    targetValue,
                                    fadeDuration)
                                .SetEase(Ease.InOutQuad);
    }

    private void OnDestroy()
    {
        // Limpiamos la lista al destruir o cambiar de escena
        activeIntensities.Clear();
    }
}