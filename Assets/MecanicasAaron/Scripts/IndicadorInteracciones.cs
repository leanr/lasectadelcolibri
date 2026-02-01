using UnityEngine;

public class IndicadorInteracciones : MonoBehaviour
{
    [SerializeField] private SpriteRenderer indicadorRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Si no lo asignaste en el inspector, lo busca en este objeto
        if (indicadorRenderer == null)
            indicadorRenderer = GetComponent<SpriteRenderer>();

        // Apagar SOLO el indicador
        if (indicadorRenderer != null)
            indicadorRenderer.enabled = false;
    }

    public void ToggleVisibilidad(bool encender)
    {
        if (indicadorRenderer != null)
            indicadorRenderer.enabled = encender;
    }
}
